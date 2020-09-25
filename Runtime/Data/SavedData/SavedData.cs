namespace com.faith.core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SavedData<T>
    {

        #region Private Variables

        private static List<string> _listOfKeys = new List<string>();

        private string _key;

        private PlayerPrefData<T> _playerPrefData;

        #endregion


        #region Public Callback

        public SavedData(string key, T value, Action<T> OnValueChanged) {

            _key = key;

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
                    _playerPrefData = new PlayerPrefData<T>(key, value, OnValueChanged);
                    break;
                case CoreEnums.DataSavingMode.BinaryFormater:

                    break;
            }
        }

        public string GetKey() {

            return _key;
        }

        public void SetData(T value) {

            switch (GameConfiguratorManager.dataSavingMode)
            {

                case CoreEnums.DataSavingMode.PlayerPrefsData:

                    _playerPrefData.SetData(value);

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

                    return default(T);

                default:

                    return default(T);
            }
        }

        #endregion
    }
}
