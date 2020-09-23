namespace com.faith.core
{
    using System;

    [Serializable]
    public class SavedData<T>
    {
        #region Public Variables

        public event Action<T> OnValueChangedEvent;

        #endregion
    }
}


