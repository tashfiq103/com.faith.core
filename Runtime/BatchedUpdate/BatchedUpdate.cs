using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(BatchUpdateConstant.EXECUTION_ORDER_FOR_BATCHUPDATE)]
public class BatchedUpdate : MonoBehaviour
{

    #region Custom Variables

    

    private class UpdateInfo
    {
        public int instanceIndex;
        public int bucketIndex;
    }

    private class BatchUpdateInstance
    {
        #region Custom Variables

        private class BatchUpdateBucket
        {
            #region Public Variables

            private int _remainingIntervalToUpdate;
            private int _interval;

            private HashSet<IBatchUpdateHandler> _hashSetForIBatchHandler;
            private Queue<IBatchUpdateHandler> _queueToRemoveFromHashSet = new Queue<IBatchUpdateHandler>();

            #endregion

            #region Public Callback

            public BatchUpdateBucket(int currentInterval, int interval) {


                _remainingIntervalToUpdate  = currentInterval;
                _interval                   = interval;

                _hashSetForIBatchHandler = new HashSet<IBatchUpdateHandler>();
            }

            public void AddToBucket(IBatchUpdateHandler batchUpdateHandler) {

                _hashSetForIBatchHandler.Add(batchUpdateHandler);
            }

            public void RemoveFromBucket(IBatchUpdateHandler batchUpdateHandler)
            {
                _queueToRemoveFromHashSet.Enqueue(batchUpdateHandler);
            }

            public void RequestToUpdateBatchBucket() {

                _remainingIntervalToUpdate--;

                if (_remainingIntervalToUpdate <= 0) {

                    foreach (IBatchUpdateHandler batchUpdateHandler in _hashSetForIBatchHandler)
                        batchUpdateHandler.OnBatchedUpdate();
               
                    _remainingIntervalToUpdate = _interval;
                }

                if (_queueToRemoveFromHashSet.Count > 0) {

                    while (_queueToRemoveFromHashSet.Count > 0) {

                        _hashSetForIBatchHandler.Remove(_queueToRemoveFromHashSet.Dequeue());
                    }
                }
            }

            public void Clear() {

                _hashSetForIBatchHandler.Clear();
                _queueToRemoveFromHashSet.Clear();
            }

            #endregion
        }

        #endregion

        #region Private Variables

        private int _numberOfBucket;

        private int _currentBucketIndex;

        private BatchUpdateBucket[] _batchUpdateBuckets;

        #endregion

        #region Public Callback

        public BatchUpdateInstance(int interval)
        {
            _currentBucketIndex = 0;

            _numberOfBucket = interval;

            _batchUpdateBuckets = new BatchUpdateBucket[interval];
            for (int i = 0; i < interval; i++) {

                _batchUpdateBuckets[i] = new BatchUpdateBucket(
                    currentInterval : i + 1,
                    interval : interval);

            }
        }

        public int AddToBucket(IBatchUpdateHandler batchUpdateHandler) {

            int bucketIndex = _currentBucketIndex;
            _batchUpdateBuckets[bucketIndex].AddToBucket(batchUpdateHandler);

            _currentBucketIndex++;
            if (_currentBucketIndex >= _numberOfBucket) _currentBucketIndex = 0;

            return bucketIndex;

        }

        public void RemoveFromBucket(IBatchUpdateHandler batchUpdateHandler, int bucketIndex) {

            _batchUpdateBuckets[bucketIndex].RemoveFromBucket(batchUpdateHandler);
        }

        public void RequestToUpdateBatchInstance() {

            for (int i = 0; i < _numberOfBucket; i++)
                _batchUpdateBuckets[i].RequestToUpdateBatchBucket();
        }

        public void Clear() {

            for (int i = 0; i < _numberOfBucket; i++)
                _batchUpdateBuckets[i].Clear();
        }

        #endregion
    }

    #endregion

    #region Public Variables

    public static BatchedUpdate Instance;

    #endregion

    #region Private Variables

    private int _numberOfInstances;
    private Dictionary<IBatchUpdateHandler, UpdateInfo> batchUpdateHandlerTracker = new Dictionary<IBatchUpdateHandler, UpdateInfo>();
    private BatchUpdateInstance[] _batchUpdateInstances;

    #endregion

    #region Mono Behaviour

    private void Awake()
    {

        List<int> listOfIntervals = new List<int>();
        foreach (int value in Enum.GetValues(typeof(BatchInterval)))
            listOfIntervals.Add(value);

        _numberOfInstances = listOfIntervals.Count;
        _batchUpdateInstances = new BatchUpdateInstance[_numberOfInstances];
        for (int i = 0; i < _numberOfInstances; i++)
            _batchUpdateInstances[i] = new BatchUpdateInstance(listOfIntervals[i]);

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Update()
    {
        for (int i = 0; i < _numberOfInstances; i++)
            _batchUpdateInstances[i].RequestToUpdateBatchInstance();
    }



    #endregion

    #region Configuretion

    private void OnSceneUnloaded(Scene scene) {

        Debug.Log("SceneUnloaded");

        batchUpdateHandlerTracker.Clear();
        for (int i = 0; i < _numberOfInstances; i++) {

            _batchUpdateInstances[i].Clear();
        }
    }

    #endregion

    #region Public Callback

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateSingletonReference() {

        if (Instance == null) {

            GameObject batchUpdateObject = new GameObject("BatchUpdate");
            BatchedUpdate batchedUpdate = batchUpdateObject.AddComponent<BatchedUpdate>();

            Instance = batchedUpdate;
            DontDestroyOnLoad(batchUpdateObject);
        }
    }

    public void RegisterToBatchedUpdate(IBatchUpdateHandler batchUpdateHandler, BatchInterval batchInterval) {

        if (!batchUpdateHandlerTracker.ContainsKey(batchUpdateHandler))
        {
            int indexOfBatchUpdateInstance = 0;
            foreach (int value in Enum.GetValues(typeof(BatchInterval)))
            {

                if (value == (int)batchInterval)
                    break;

                indexOfBatchUpdateInstance++;
            }

            int indexOfBatchBucketOnInstance = _batchUpdateInstances[indexOfBatchUpdateInstance].AddToBucket(batchUpdateHandler);

            batchUpdateHandlerTracker.Add(
                    batchUpdateHandler,
                    new UpdateInfo()
                    {
                        instanceIndex = indexOfBatchUpdateInstance,
                        bucketIndex = indexOfBatchBucketOnInstance
                    }
                );
        }
    }

    public void UnregisterFromBatchedUpdate(IBatchUpdateHandler batchUpdateHandler) {

        UpdateInfo updateInfo = null;
        batchUpdateHandlerTracker.TryGetValue(batchUpdateHandler, out updateInfo);

        if (updateInfo != null) {

            _batchUpdateInstances[updateInfo.instanceIndex].RemoveFromBucket(batchUpdateHandler, updateInfo.bucketIndex);
            batchUpdateHandlerTracker.Remove(batchUpdateHandler);
        }
    }

    #endregion
}
