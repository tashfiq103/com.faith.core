namespace com.faith.core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SavedData<T>
    {
        #region Public Variables

        //Section   :   BinaryFormat
        public event Action<T> OnValueChangedEvent;

        #endregion

        #region Private Variables

        private static List<string> _listOfKeys = new List<string>();

        private string _key;

        //Section   :   PlayerPrefsData
        private PlayerPrefData<T> _playerPrefData;

        //Section   :   BinaryFormat
        private CoreEnums.DataTypeForSavingData _dataType;
        private T _value;

        #endregion

        #region Configuretion

        private bool AssigningDataType(T value)
        {

            //Assigning :   DataType
            if (typeof(T) == typeof(bool))
            {
                _dataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_BOOL;
                return true;
            }
            else if (typeof(T) == typeof(int))
            {
                _dataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_INT;
                return true;
            }
            else if (typeof(T) == typeof(float))
            {
                _dataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_FLOAT;
                return true;
            }
            else if (typeof(T) == typeof(double))
            {
                _dataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_DOUBLE;
                return true;
            }
            else if (typeof(T) == typeof(string))
            {

                _dataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_STRING;
                return true;
            }

            CoreDebugger.Debug.LogError("Invalid DataType for Value : " + value);
            _dataType = CoreEnums.DataTypeForSavingData.UNDEFINED;
            return false;
        }

        #endregion

        #region Public Callback

        public SavedData(string key, T value, Action<T> OnValueChanged) {

            _key = key;
            _value = value;
            AssigningDataType(_value);

            if (_listOfKeys.Contains(_key))
            {
                CoreDebugger.Debug.LogError("Key : " + _key + ", is already in used!. Please generate unique key for this data");
            }
            else
            {
                _listOfKeys.Add(_key);
            }

            switch (GameConfiguratorManager.dataSavingMode) {

                case CoreEnums.DataSavingMode.PlayerPrefsData:
                    _playerPrefData = new PlayerPrefData<T>(key, _value, OnValueChanged);
                    break;
                case CoreEnums.DataSavingMode.BinaryFormater:

                    break;
            }
        }

        public string GetKey() {

            return _key;
        }

        public void SetData(T value) {

            _value = value;

            switch (GameConfiguratorManager.dataSavingMode)
            {

                case CoreEnums.DataSavingMode.PlayerPrefsData:

                    _playerPrefData.SetData(_value);

                    break;
                case CoreEnums.DataSavingMode.BinaryFormater:



                    break;
            }
        }

        public T GetData() {

            switch (GameConfiguratorManager.dataSavingMode)
            {

                case CoreEnums.DataSavingMode.PlayerPrefsData:

                    return _playerPrefData.GetData();

                case CoreEnums.DataSavingMode.BinaryFormater:

                    return _value;

                default:

                    return _value;
            }
        }

        #endregion
    }
}
