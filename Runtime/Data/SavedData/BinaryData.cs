namespace com.faith.core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class BinaryData<T> : BaseDataClass<T>
    {
        #region Private Variables

        private static List<string> _listOfKeys = new List<string>();

        #endregion

        #region Public Callback

        public BinaryData(string key, T value, Action<T> OnValueChanged)
        {

            base.key = key;

            RegisterOnValueChangedEvent(OnValueChanged);

            if (!_listOfKeys.Contains(key))
            {
                SetData(value);
            }
            else {

                if (AssigningDataType(value)) {

                    InvokeOnValueChangedEvent(GetData());
                }
            }
            
        }

        public void RegisterOnValueChangedEvent(Action<T> OnValueChanged) {

            if (OnValueChanged != null)
            {
                OnValueChangedEvent += OnValueChanged;
                InvokeOnValueChangedEvent(GetData());
            }
        }

        public void SetData(T value) {

        }

        public T GetData() {

            return default(T);
        }

        #endregion
    }
}

