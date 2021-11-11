namespace com.faith.core
{
    using UnityEngine.Events;
    public class BatchedUpdateThread : IBatchedUpdateHandler
    {
        public bool IsUpdateThreadRunning { get; private set; } = false;
        public UnityAction Update;

        public BatchedUpdateThread(UnityAction Update)
        {
            this.Update = Update;
        }

        public void StartUpdate(int batchInterval = 1)
        {
            BatchedUpdate.Instance.RegisterToBatchedUpdate(this, batchInterval);
            IsUpdateThreadRunning = true;
        }

        public void StopUpdate()
        {
            IsUpdateThreadRunning = false;
            BatchedUpdate.Instance.UnregisterFromBatchedUpdate(this);
        }

        public void OnBatchedUpdate()
        {
            Update?.Invoke();
        }
    }
}


