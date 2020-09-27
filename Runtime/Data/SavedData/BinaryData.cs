namespace com.faith.core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class BinaryData<T> : BaseDataClass<T>
    {
        #region Private Variables

        private static List<string> _listOfKeys = new List<string>();

        private int _indexOnBinaryDataWrapper = -1;
        private T _intializedValue;

        #endregion

        #region Public Callback

        public BinaryData(string key, T value, Action<T> OnValueChanged = null)
        {

            base.key = key;
            _intializedValue = value;
            RegisterOnValueChangedEvent(OnValueChanged);

            if (!_listOfKeys.Contains(key))
            {
                BinaryFormatedData.RegisterInBinaryData(this);
            }
            else {

                if (AssigningDataType(value)) {

                    BinaryFormatedData.RegisterInBinaryData(this);
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

        public void SetIndexOfBinaryDataWrapper(int index) {

            _indexOnBinaryDataWrapper = index;
        }

        public void SetData(T value) {

            BinaryFormatedData.SetData(_indexOnBinaryDataWrapper, value);
            InvokeOnValueChangedEvent(GetData());
        }

        public T GetData() {

            return BinaryFormatedData.GetData<T>(ref _indexOnBinaryDataWrapper);
        }

        public T GetInitializedValue() {

            return _intializedValue;
        }

        #endregion
    }
}

