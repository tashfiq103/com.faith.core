using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(BatchedUpdateConstant.EXECUTION_ORDER_FOR_BATCHUPDATE)]
public class BatchedUpdate : MonoBehaviour
{
    #region Custom Variables

    private class UpdateInfo
    {
        public int instanceIndex;
        public int bucketIndex;
    }

    public class BatchUpdateInstance
    {
        #region Custom Variables

        private class BatchUpdateBucket
        {
            #region Public Variables

            private int _remainingIntervalToUpdate;
            private int _interval;

            private HashSet<IBatchedUpdateHandler> _hashSetForIBatchHandler;
            private Queue<IBatchedUpdateHandler> _queueToRemoveFromHashSet = new Queue<IBatchedUpdateHandler>();

            #endregion

            #region Public Callback

            public BatchUpdateBucket(int currentInterval, int interval)
            {


                _remainingIntervalToUpdate = currentInterval;
                _interval = interval;

                _hashSetForIBatchHandler = new HashSet<IBatchedUpdateHandler>();
            }

            public void AddToBucket(IBatchedUpdateHandler batchUpdateHandler)
            {
                _hashSetForIBatchHandler.Add(batchUpdateHandler);
                Debug.Log("Added");
            }

            public void RemoveFromBucket(IBatchedUpdateHandler batchUpdateHandler)
            {
                _queueToRemoveFromHashSet.Enqueue(batchUpdateHandler);
            }

            public void RequestToUpdateBatchBucket()
            {

                _remainingIntervalToUpdate--;

                if (_remainingIntervalToUpdate <= 0)
                {

                    foreach (IBatchedUpdateHandler batchUpdateHandler in _hashSetForIBatchHandler)
                        batchUpdateHandler.OnBatchedUpdate();

                    _remainingIntervalToUpdate = _interval;
                }

                if (_queueToRemoveFromHashSet.Count > 0)
                {

                    while (_queueToRemoveFromHashSet.Count > 0)
                    {

                        _hashSetForIBatchHandler.Remove(_queueToRemoveFromHashSet.Dequeue());
                    }
                }
            }

            public void Clear()
            {

                _hashSetForIBatchHandler.Clear();
                _queueToRemoveFromHashSet.Clear();
            }

            #endregion
        }

        #endregion

        #region Public Variables

        public int Interval { get; private set; }

        #endregion

        #region Private Variables

        private int _currentBucketIndex;
        private int _currentNumberOfBucket;
        private List<BatchUpdateBucket> _batchUpdateBuckets;

        #endregion

        #region Public Callback

        public BatchUpdateInstance(int interval)
        {
            Interval = interval;

            _currentBucketIndex     = 0;
            _currentNumberOfBucket  = 0;
            _batchUpdateBuckets     = new List<BatchUpdateBucket>();

        }

        public int AddToBucket(IBatchedUpdateHandler batchUpdateHandler)
        {
            int currentBucketIndex = _currentBucketIndex;
            if (currentBucketIndex >= _currentNumberOfBucket && _currentNumberOfBucket < Interval)
            {
                _batchUpdateBuckets.Add(new BatchUpdateBucket(currentBucketIndex, Interval));
                _currentNumberOfBucket++;
            }

            
            _batchUpdateBuckets[currentBucketIndex].AddToBucket(batchUpdateHandler);

            _currentBucketIndex++;
            if (_currentBucketIndex >= Interval) 
                _currentBucketIndex = 0;

            return currentBucketIndex;

        }

        public void RemoveFromBucket(IBatchedUpdateHandler batchUpdateHandler, int bucketIndex)
        {
            _batchUpdateBuckets[bucketIndex].RemoveFromBucket(batchUpdateHandler);
        }

        public void RequestToUpdateBatchInstance()
        {

            for (int i = 0; i < _currentNumberOfBucket; i++)
                _batchUpdateBuckets[i].RequestToUpdateBatchBucket();
        }

        public void Clear()
        {

            for (int i = 0; i < _currentNumberOfBucket; i++)
                _batchUpdateBuckets[i].Clear();
        }

        #endregion
    }

    #endregion

    #region Public Variables

    public static BatchedUpdate Instance;
    public List<BatchUpdateInstance> BatchUpdateInstances { get; private set; } = new List<BatchUpdateInstance>();

    #endregion

    #region Private Variables

    private int _numberOfInstances;
    private Dictionary<IBatchedUpdateHandler, UpdateInfo> batchUpdateHandlerTracker   = new Dictionary<IBatchedUpdateHandler, UpdateInfo>();

    #endregion

    #region Mono Behaviour

    private void Awake()
    {

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Update()
    {
        for (int i = 0; i < _numberOfInstances; i++)
            BatchUpdateInstances[i].RequestToUpdateBatchInstance();
    }

    #endregion

    #region Configuretion

    private int HasThisInstance(int interval) {

        int numberOfInstances = BatchUpdateInstances.Count;
        for (int i = 0; i < numberOfInstances; i++) {

            if (BatchUpdateInstances[i].Interval == interval)
                return i;
        }

        return -1;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        int numberOfInstances = BatchUpdateInstances.Count;
        batchUpdateHandlerTracker.Clear();
        for (int i = 0; i < numberOfInstances; i++)
            BatchUpdateInstances[i].Clear();

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateSingletonReference()
    {
        if (Instance == null)
        {

            GameObject batchUpdateObject    = new GameObject("BatchUpdate");
            BatchedUpdate batchedUpdate       = batchUpdateObject.AddComponent<BatchedUpdate>();

            Instance = batchedUpdate;
            DontDestroyOnLoad(batchUpdateObject);
        }
    }

    #endregion

    #region Public Callback

    public void RegisterToBatchedUpdate(IBatchedUpdateHandler batchUpdateHandler, int batchInterval) {

        // if : It doesn't have this Interface
        if (!batchUpdateHandlerTracker.ContainsKey(batchUpdateHandler)) {

            int instanceIndex = HasThisInstance(batchInterval);
            
            if (instanceIndex == -1) {

                BatchUpdateInstances.Add(new BatchUpdateInstance(batchInterval));
                instanceIndex = BatchUpdateInstances.Count - 1;
                _numberOfInstances++;
            }

            int bucketIndex = BatchUpdateInstances[instanceIndex].AddToBucket(batchUpdateHandler);
            batchUpdateHandlerTracker.Add(
                    batchUpdateHandler,
                    new UpdateInfo()
                    {
                        instanceIndex = instanceIndex,
                        bucketIndex = bucketIndex
                    }
                );

            return;
        }

        Debug.LogError(string.Format("{0} is already in registered in BatchUpdate", batchUpdateHandler));
    }

    public void UnregisterFromBatchedUpdate(IBatchedUpdateHandler batchUpdateHandler) {

        UpdateInfo updateInfo = null;
        batchUpdateHandlerTracker.TryGetValue(batchUpdateHandler, out updateInfo);

        if (updateInfo != null)
        {
            BatchUpdateInstances[updateInfo.instanceIndex].RemoveFromBucket(batchUpdateHandler, updateInfo.bucketIndex);
            batchUpdateHandlerTracker.Remove(batchUpdateHandler);
        }
    }

    #endregion
}
