using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public class BatchUpdateBucket
        {
            #region Public Variables

            public int NumberOfBatchedUpdateHandlerInBucket { get; private set; } = 0;

            #endregion

            #region Private Variables

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

            public bool AddToBucket(IBatchedUpdateHandler batchUpdateHandler)
            {
                if (_hashSetForIBatchHandler.Add(batchUpdateHandler)) {

                    NumberOfBatchedUpdateHandlerInBucket++;
                    return true;
                }

                return false;
            }

            public bool RemoveFromBucket(IBatchedUpdateHandler batchUpdateHandler)
            {
                bool hasContain = _hashSetForIBatchHandler.Contains(batchUpdateHandler);

                if(hasContain)
                    _queueToRemoveFromHashSet.Enqueue(batchUpdateHandler);

                return hasContain;
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
                        NumberOfBatchedUpdateHandlerInBucket--;
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
        public int NumberOfActiveBucket { get; private set; }
        public List<BatchUpdateBucket> BatchUpdateBuckets { get; private set; }

        #endregion

        #region Private Variables

        private int _currentBucketIndex;
        
        

        private Queue<BatchUpdateBucket> _queueToRemoveBucket;

        #endregion

        #region Public Callback

        public BatchUpdateInstance(int interval)
        {
            Interval = interval;
            NumberOfActiveBucket = 0;

            _currentBucketIndex     = 0;
            BatchUpdateBuckets     = new List<BatchUpdateBucket>();
            _queueToRemoveBucket    = new Queue<BatchUpdateBucket>();

        }

        public int AddToBucket(IBatchedUpdateHandler batchUpdateHandler)
        {
            int currentBucketIndex = _currentBucketIndex;
            if (currentBucketIndex >= NumberOfActiveBucket && NumberOfActiveBucket < Interval)
            {
                BatchUpdateBuckets.Add(new BatchUpdateBucket(currentBucketIndex, Interval));
                NumberOfActiveBucket++;
            }

            
            BatchUpdateBuckets[currentBucketIndex].AddToBucket(batchUpdateHandler);

            _currentBucketIndex++;
            if (_currentBucketIndex >= Interval) 
                _currentBucketIndex = 0;

            return currentBucketIndex;

        }

        public bool RemovedFromBucket(IBatchedUpdateHandler batchUpdateHandler, int bucketIndex)
        {
            Debug.Log(string.Format("bucketIndex = {0}", bucketIndex));
            if (BatchUpdateBuckets[bucketIndex].RemoveFromBucket(batchUpdateHandler)) {

                if (BatchUpdateBuckets[bucketIndex].NumberOfBatchedUpdateHandlerInBucket == 0) 
                    _queueToRemoveBucket.Enqueue(BatchUpdateBuckets[bucketIndex]);
                
                return true;
            }

            return false;
        }

        public void RequestToUpdateBatchInstance()
        {
            if (_queueToRemoveBucket.Count > 0)
            {
                while (_queueToRemoveBucket.Count > 0)
                {
                    BatchUpdateBuckets.Remove(_queueToRemoveBucket.Dequeue());
                    NumberOfActiveBucket--;

                }
            }

            for (int i = 0; i < NumberOfActiveBucket; i++)
            {
                BatchUpdateBuckets[i].RequestToUpdateBatchBucket();
            }
        }

        public void Clear()
        {

            for (int i = 0; i < NumberOfActiveBucket; i++)
                BatchUpdateBuckets[i].Clear();
        }

        #endregion
    }

    #endregion

    #region Public Variables

    public static BatchedUpdate Instance;
    public int NumberOfInstances { get; private set; }
    public List<BatchUpdateInstance> BatchUpdateInstances { get; private set; } = new List<BatchUpdateInstance>();

    #endregion

    #region Private Variables

    private OrderedDictionary batchUpdateHandlerTracker = new OrderedDictionary();
    //private Dictionary<IBatchedUpdateHandler, UpdateInfo> batchUpdateHandlerTracker     = new Dictionary<IBatchedUpdateHandler, UpdateInfo>();
    private Queue<BatchUpdateInstance> _queueForRemovingUpdateInstances                 = new Queue<BatchUpdateInstance>();

    #endregion

    #region Mono Behaviour

    private void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Update()
    {
        while (_queueForRemovingUpdateInstances.Count > 0)
        {
            BatchUpdateInstances.Remove(_queueForRemovingUpdateInstances.Dequeue());
            BatchUpdateInstances.TrimExcess();

            NumberOfInstances--;
            ShiftInstanceIndexForRemoving();
        }

        for (int i = 0; i < NumberOfInstances; i++)
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

    private int HasTheKeyInBatchUpdateTracker(IBatchedUpdateHandler batchedUpdateHandlerReference) {

        int index  = 0;
        ICollection keyValues = batchUpdateHandlerTracker.Keys;
        foreach (IBatchedUpdateHandler batchedUpdateHandler in keyValues) {

            if (batchedUpdateHandlerReference == batchedUpdateHandler)
                return index;

            index++;
        }

        return -1;
    }

    private UpdateInfo GetUpdateInfoInBatchedUpdateTracker(int index) {

        ICollection values = batchUpdateHandlerTracker.Values;
        int numberOfValues = values.Count;

        UpdateInfo[] updateInfos = new UpdateInfo[values.Count];
        values.CopyTo(updateInfos, 0);

        return (index >= 0 && index < numberOfValues) ? updateInfos[index] : null;
    }

    private void ShiftInstanceIndexForRemoving() {

        int numberOfItem    = batchUpdateHandlerTracker.Count;

        IBatchedUpdateHandler[] batchedUpdateHandlers = new IBatchedUpdateHandler[numberOfItem];
        ICollection keys    = batchUpdateHandlerTracker.Keys;
        keys.CopyTo(batchedUpdateHandlers, 0);

        ICollection values  = batchUpdateHandlerTracker.Values;
        UpdateInfo[] updateInfos = new UpdateInfo[numberOfItem];
        values.CopyTo(updateInfos, 0);

        for (int i = 0; i < numberOfItem; i++) {

            if (updateInfos[i].instanceIndex > 0) {

                updateInfos[i].instanceIndex--;
                batchUpdateHandlerTracker[batchedUpdateHandlers[i]] = updateInfos[i];
            }
        }
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

        batchInterval = Mathf.Clamp(batchInterval, 1, int.MaxValue);

        // if : It doesn't have this Interface
        if (!batchUpdateHandlerTracker.Contains(batchUpdateHandler)) {

            int instanceIndex = HasThisInstance(batchInterval);
            
            if (instanceIndex == -1) {

                BatchUpdateInstances.Add(new BatchUpdateInstance(batchInterval));
                instanceIndex = BatchUpdateInstances.Count - 1;
                NumberOfInstances++;
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

        UpdateInfo updateInfo = GetUpdateInfoInBatchedUpdateTracker(HasTheKeyInBatchUpdateTracker(batchUpdateHandler));
        
        if (updateInfo != null)
        {
            Debug.Log(string.Format("Requested :  UpdateInfo [instance = {0}, bucketIndex = {1}]", updateInfo.instanceIndex, updateInfo.bucketIndex));
            if (BatchUpdateInstances[updateInfo.instanceIndex].RemovedFromBucket(batchUpdateHandler, updateInfo.bucketIndex)) {

                batchUpdateHandlerTracker.Remove(batchUpdateHandler);

                Debug.Log(string.Format("Acomplished :  UpdateInfo [instance = {0}, bucketIndex = {1}]", updateInfo.instanceIndex, updateInfo.bucketIndex));

                if (BatchUpdateInstances[updateInfo.instanceIndex].NumberOfActiveBucket == 0) {
                    _queueForRemovingUpdateInstances.Enqueue(BatchUpdateInstances[updateInfo.instanceIndex]);
                }
            }
            
        }
    }

    #endregion
}
