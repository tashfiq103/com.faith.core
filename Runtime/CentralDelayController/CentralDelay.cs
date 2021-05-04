namespace com.faith.core
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    [DefaultExecutionOrder(CentralDelayConstant.EXECUTION_ORDER_FOR_CENTRALDELAY)]
    public class CentralDelay : MonoBehaviour
    {

        #region Custom Variables

        public class DelayInstance : IBatchedUpdateHandler
        {
            #region Public Variables

            public bool CancelOnSceneLoad { get; private set; }

            #endregion

            #region Private Variables

            private float _remainingDelay;
            private float _delay;
            private UnityAction<float> _OnProgression;
            private UnityAction _OnDelayEnd;
            #endregion

            #region Configuretion

            private void Initialization(float delay, bool cancelOnSceneLoad = true, UnityAction<float> OnProgression = null, UnityAction OnDelayEnd = null)
            {

                _remainingDelay = delay;
                _delay = delay;

                CancelOnSceneLoad = cancelOnSceneLoad;

                _OnProgression = OnProgression;
                _OnDelayEnd = OnDelayEnd;

                BatchedUpdate.Instance.RegisterToBatchedUpdate(this, 1);
            }

            #endregion

            #region Public Callback

            public DelayInstance(float delay, bool cancelOnSceneLoad = true, UnityAction OnDelayEnd = null)
            {
                Initialization(delay, cancelOnSceneLoad, null, OnDelayEnd);
            }

            public DelayInstance(float delay, bool cancelOnSceneLoad = true, UnityAction<float> OnProgression = null, UnityAction OnDelayEnd = null)
            {
                Initialization(delay, cancelOnSceneLoad, OnProgression, OnDelayEnd);
            }

            public void ForceUnregister()
            {

                BatchedUpdate.Instance.UnregisterFromBatchedUpdate(this);
            }

            public void OnBatchedUpdate()
            {
                _remainingDelay -= Time.deltaTime;

                _OnProgression?.Invoke(_remainingDelay / _delay);

                if (_remainingDelay <= 0)
                {
                    _OnDelayEnd?.Invoke();
                    ForceUnregister();
                }
            }

            #endregion

        }

        #endregion

        #region Public Variables

        public static CentralDelay Instance;

        #endregion

        #region Private Variables

        private List<DelayInstance> _listOfDelayInstance = new List<DelayInstance>();

        #endregion

        #region Configuretion

        private void OnSceneUnloaded(Scene scene)
        {

            List<DelayInstance> listOfInstancesToBeRemoved = new List<DelayInstance>();
            int numberOfDelayInstance = _listOfDelayInstance.Count;
            for (int i = 0; i < numberOfDelayInstance; i++)
            {

                if (_listOfDelayInstance[i].CancelOnSceneLoad)
                    listOfInstancesToBeRemoved.Add(_listOfDelayInstance[i]);
            }

            foreach (DelayInstance delayInstance in listOfInstancesToBeRemoved)
            {

                delayInstance.ForceUnregister();
                _listOfDelayInstance.Remove(delayInstance);
            }
        }

        #endregion

        #region Mono Behaviour

        private void Awake()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        #endregion

        #region Public Callback

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateSingletonReference()
        {
            if (Instance == null)
            {

                GameObject centralDelayObject = new GameObject("CentralDelay");
                CentralDelay batchedUpdate = centralDelayObject.AddComponent<CentralDelay>();

                Instance = batchedUpdate;
                DontDestroyOnLoad(centralDelayObject);
            }
        }


        public DelayInstance SetDelay(float delay, bool cancelOnSceneLoad = true, UnityAction<float> OnProgression = null, UnityAction OnDelayEnd = null)
        {

            DelayInstance newDelayInstance = new DelayInstance(
                    delay,
                    cancelOnSceneLoad,
                    OnProgression,
                    OnDelayEnd
                );
            _listOfDelayInstance.Add(newDelayInstance);

            return newDelayInstance;
        }

        #endregion

    }
}

