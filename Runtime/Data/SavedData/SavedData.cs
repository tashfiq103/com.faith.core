namespace com.faith.core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SavedData<T>
    {

        #region Private Variables

        private static List<string> _listOfKeys = new List<string>();

        private PlayerPrefData<T> _playerPrefData;
        private BinaryData<T> _binaryData;

        #endregion


        #region Public Callback

        public SavedData(string key, T value, Action<T> OnValueChanged = null) {

            if (_listOfKeys.Contains(key))
            {
                //CoreDebugger.Debug.LogWarning("Key : " + key + ", is already in used!. Please generate unique key for this data");
            }
            else
            {
                _listOfKeys.Add(key);
            }

            switch (GameConfiguratorManager.dataSavingMode) {

                case CoreEnums.DataSavingMode.PlayerPrefsData:
                    _playerPrefData = new PlayerPrefData<T>(key, value, OnValueChanged);
                    break;
                case CoreEnums.DataSavingMode.BinaryFormater:
                    _binaryData = new BinaryData<T>(key, value, OnValueChanged);
                    break;
            }
        }

        public void SetData(T value) {

            switch (GameConfiguratorManager.dataSavingMode)
            {

                case CoreEnums.DataSavingMode.PlayerPrefsData:

                    _playerPrefData.SetData(value);

                    break;
                case CoreEnums.DataSavingMode.BinaryFormater:

                    _binaryData.SetData(value);

                    break;
            }
        }

        public T GetData() {

            switch (GameConfiguratorManager.dataSavingMode)
            {

                case CoreEnums.DataSavingMode.PlayerPrefsData:

                    return _playerPrefData.GetData();

                case CoreEnums.DataSavingMode.BinaryFormater:

                    return _binaryData.GetData();

                default:

                    return default(T);
            }
        }

        #endregion
    }
}
