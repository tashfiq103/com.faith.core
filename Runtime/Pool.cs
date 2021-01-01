namespace com.faith.core
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    public class Pool
    {
        #region Static Field

        private static Pool _GlobalPoolManager = null;
        public static Pool GlobalPoolManager
        { get
            {
                if (_GlobalPoolManager == null)
                    _GlobalPoolManager = new Pool("Global");

                return _GlobalPoolManager;
            }
        }

        #endregion

        #region Custom Variables


        private class PoolType
        {
            #region Public Variables

            public GameObject prefabOrigin { get; private set; }
            public int NumberOfPoolItem { get { return _listOfPoolItems.Count; } }

            #endregion

            #region Private Variables

            private Transform           _parentForUnamagedPoolType;
            private List<GameObject>    _listOfPoolItems;

            #endregion

            #region Public Callback

            public PoolType(GameObject prefabOrigin, Transform _rootParent)
            {
                this.prefabOrigin = prefabOrigin;
                _parentForUnamagedPoolType = new GameObject("Parent - " + this.prefabOrigin.name).GetComponent<Transform>();
                _parentForUnamagedPoolType.SetParent(_rootParent);
                _listOfPoolItems            = new List<GameObject>();
            }

            public void Reset() {

                foreach (GameObject gameObjectReference in _listOfPoolItems)
                {
                    if (gameObjectReference != null)
                        MonoBehaviour.Destroy(gameObjectReference);
                }
                _listOfPoolItems = null;

                if(_parentForUnamagedPoolType != null)
                    MonoBehaviour.Destroy(_parentForUnamagedPoolType.gameObject);
                
            }

            public GameObject GetPoolItem(GameObject prefabOrigin, Vector3 position, Quaternion rotation, Transform parent = null) {

                foreach (GameObject gameObjectReference in _listOfPoolItems) {

                    if (!gameObjectReference.activeSelf) {

                        Transform transformReferenceOfPoolItem = gameObjectReference.transform;

                        if (parent == null)
                        {
                            transformReferenceOfPoolItem.SetParent(_parentForUnamagedPoolType);
                            transformReferenceOfPoolItem.position = position;
                            transformReferenceOfPoolItem.rotation = rotation;
                        }
                        else {

                            transformReferenceOfPoolItem.SetParent(parent);
                            transformReferenceOfPoolItem.localPosition = position;
                            transformReferenceOfPoolItem.localRotation = rotation;
                        }

                        gameObjectReference.SetActive(true);

                        return gameObjectReference;
                    }
                        
                }

                //if : No Object Available
                Transform transformReferenceOfNewPoolItem;
                if (parent == null)
                {
                    transformReferenceOfNewPoolItem = MonoBehaviour.Instantiate(_parentForUnamagedPoolType).transform;
                    transformReferenceOfNewPoolItem.position = position;
                    transformReferenceOfNewPoolItem.rotation = rotation;
                }
                else {

                    transformReferenceOfNewPoolItem = MonoBehaviour.Instantiate(parent).transform;
                    transformReferenceOfNewPoolItem.position = position;
                    transformReferenceOfNewPoolItem.rotation = rotation;
                }

                _listOfPoolItems.Add(transformReferenceOfNewPoolItem.gameObject);

                return transformReferenceOfNewPoolItem.gameObject;
            }

            public bool PushPoolItem(GameObject gameObjectReference) {

                foreach (GameObject gameObjectReferenceInList in _listOfPoolItems)
                {
                    if (gameObjectReferenceInList == gameObjectReference) {

                        gameObjectReference.SetActive(false);
                        return true;
                    }
                }

                return false;
            }

            public bool RemovePoolItem(GameObject gameObjectReference)
            {
                if (_listOfPoolItems.Remove(gameObjectReference)) {

                    MonoBehaviour.Destroy(gameObjectReference);
                    return true;
                }

                return false;
            }

            #endregion

        }

        #endregion

        #region Private Variables


        private  Transform        _rootParentForUnmanagedPoolItem;
        private  List<PoolType>   _listForPoolTypes;

        #endregion

        #region Configuretion

        public Pool(string prefix) {

            _rootParentForUnmanagedPoolItem = new GameObject("PoolManager - " + prefix).GetComponent<Transform>();
            _listForPoolTypes               = new List<PoolType>();

            SceneManager.sceneLoaded    += OnSceneLoaded;
            SceneManager.sceneUnloaded  += OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {

            ClearAll();
        }
        private void OnSceneUnloaded(Scene scene) {

            
        }

        #endregion

        #region Public Callback
        /// <summary>
        /// if parent == null, it will instanciate under the unmanaged pool heirarchy and the position & rotation would be set to world point\n
        /// else, it will be spawn under the assigned parent and the position & rotation will be local.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public  GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        /// <summary>
        /// if parent == null, it will instanciate under the unmanaged pool heirarchy and the position & rotation would be set to world point\n
        /// else, it will be spawn under the assigned parent and the position & rotation will be local.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            bool createPoolType = true;
            PoolType poolType   = null;
            foreach (PoolType poolTypeInList in _listForPoolTypes) {

                if (poolTypeInList.prefabOrigin == prefab) {
                    poolType = poolTypeInList;
                    createPoolType = false;
                    break;
                }
            }

            if (createPoolType) {
                poolType = new PoolType(prefab, _rootParentForUnmanagedPoolItem);
                _listForPoolTypes.Add(poolType);
            }

            return poolType.GetPoolItem(prefab, position, rotation, parent);
        }

        public void Destroy(GameObject gameObjectReference, bool destroyIt = false) {

            bool found = false;
            foreach (PoolType poolTypeInList in _listForPoolTypes) {

                if (destroyIt) {
                    found = poolTypeInList.RemovePoolItem(gameObjectReference);
                    if (poolTypeInList.NumberOfPoolItem == 0) {

                        poolTypeInList.Reset();
                        _listForPoolTypes.Remove(poolTypeInList);
                    }
                }
                else
                    found = poolTypeInList.PushPoolItem(gameObjectReference);

                if (found)
                    break;
            }
        }

        public bool Clear(GameObject prefabReference)
        {
            foreach (PoolType poolType in _listForPoolTypes) {

                if (poolType.prefabOrigin == prefabReference) {

                    poolType.Reset();
                    _listForPoolTypes.Remove(poolType);
                    return true;
                }
            }

            return false;
        }

        public void ClearAll() {

            foreach (PoolType poolType in _listForPoolTypes)
            {
                poolType.Reset();
            }

            _listForPoolTypes.Clear();
        }

        #endregion

    }
}

