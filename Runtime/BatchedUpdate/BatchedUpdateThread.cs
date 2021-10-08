namespace com.faith.core
{
    using UnityEngine.Events;
    public class BatchedUpdateThread : IBatchedUpdateHandler
    {
        public UnityAction Update;

        public BatchedUpdateThread(UnityAction Update)
        {
            this.Update = Update;
        }

        public void StartUpdate(int batchInterval = 1)
        {
            BatchedUpdate.Instance.RegisterToBatchedUpdate(this, batchInterval);
        }

        public void StopUpdate()
        {
            BatchedUpdate.Instance.UnregisterFromBatchedUpdate(this);
        }

        public void OnBatchedUpdate()
        {
            Update?.Invoke();
        }
    }
}


