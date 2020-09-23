namespace com.faith.core
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

#if UNITY_EDITOR

    public static class PlayerPrefDataSettings
    {

        #region Custom DataType

        [Serializable]
        public class PlayerPrefEditorData
        {
            public string key;
            public Type type;
            public string value;

            private event Action<bool>      OnValueChangedForBool;
            private event Action<int>       OnValueChangedForInt;
            private event Action<float>     OnValueChangedForFloat;
            private event Action<double>    OnValueChangedForDouble;
            private event Action<string>    OnValueChangedForString;

            public void AssignValueChangedEvent<T>(ref Action<T> OnValueChanged) {

                if (typeof(T) == typeof(bool))
                {
                    OnValueChangedForBool = (Action<bool>)Convert.ChangeType(OnValueChanged, typeof(Action<bool>));
                }
                else if (typeof(T) == typeof(int)){

                    OnValueChangedForInt = (Action<int>)Convert.ChangeType(OnValueChanged, typeof(Action<int>));
                }
                else if (typeof(T) == typeof(float)){

                    OnValueChangedForFloat = (Action<float>)Convert.ChangeType(OnValueChanged, typeof(Action<float>));
                }
                else if (typeof(T) == typeof(double)){

                    OnValueChangedForDouble = (Action<double>)Convert.ChangeType(OnValueChanged, typeof(Action<double>));
                }
                else if (typeof(T) == typeof(string)){

                    OnValueChangedForString = (Action<string>)Convert.ChangeType(OnValueChanged, typeof(Action<string>));
                }
            }

            public void InvokeEvent(string value) {

                this.value = value;

                if (type == typeof(bool)) {

                    OnValueChangedForBool?.Invoke((bool)Convert.ChangeType(value, typeof(bool)));
                }
                else if (type == typeof(int)){

                    OnValueChangedForInt?.Invoke((int)Convert.ChangeType(value, typeof(int)));
                }
                else if (type == typeof(float))
                {
                    OnValueChangedForFloat?.Invoke((float)Convert.ChangeType(value, typeof(float)));
                }
                else if (type == typeof(double))
                {
                    OnValueChangedForDouble?.Invoke((double)Convert.ChangeType(value, typeof(double)));
                }
                else if (type == typeof(string))
                {
                    OnValueChangedForString?.Invoke((string)Convert.ChangeType(value, typeof(string)));
                }
            }
        }

        #endregion

        #region Private Variables

        public static List<PlayerPrefEditorData> listOfUsedPlayerPrefEditorData = new List<PlayerPrefEditorData>();

        #endregion

        #region Public Callback

        public static int IsPlayerPrefEditorDataAlreadyInContainer(string t_Key)
        {

            int t_NumberOfEnlistedPlayerPrefEditorData = listOfUsedPlayerPrefEditorData.Count;
            for (int i = 0; i < t_NumberOfEnlistedPlayerPrefEditorData; i++)
            {

                if (string.Equals(t_Key, listOfUsedPlayerPrefEditorData[i].key))
                    return i;
            }

            return -1;
        }

        public static void EnlistPlayerPrefEditorDataInContainer<T>(string t_Key, string t_Value, ref Action<T> OnValueChanged)
        {

            int t_Result = IsPlayerPrefEditorDataAlreadyInContainer(t_Key);
            if (t_Result == -1)
            {
                PlayerPrefEditorData playerPrefEditorData = new PlayerPrefEditorData()
                {
                    key = t_Key,
                    type = typeof(T),
                    value = t_Value
                };
                playerPrefEditorData.AssignValueChangedEvent(ref OnValueChanged);

                listOfUsedPlayerPrefEditorData.Add(playerPrefEditorData);
            }
            else
            {

                listOfUsedPlayerPrefEditorData[t_Result].type = typeof(T);
                listOfUsedPlayerPrefEditorData[t_Result].value = t_Value;
            }


        }

        public static void SetData(string t_Key, Type t_Type, string t_Value)
        {
            int t_Index = IsPlayerPrefEditorDataAlreadyInContainer(t_Key);
            if (t_Index != -1)
            {

                if (t_Type == typeof(bool))
                {
                    PlayerPrefs.SetInt(t_Key, string.Compare(t_Value, "False") == 0 ? 0 : 1);
                }
                else if (t_Type == typeof(int))
                {
                    PlayerPrefs.SetInt(t_Key, (int)Convert.ChangeType(t_Value, typeof(int)));
                }
                else if (t_Type == typeof(float))
                {
                    PlayerPrefs.SetFloat(t_Key, (float)Convert.ChangeType(t_Value, typeof(float)));
                }
                else if (t_Type == typeof(double))
                {
                    PlayerPrefs.SetString(t_Key, ((double)Convert.ChangeType(t_Value, typeof(double))).ToString());
                }
                else if (t_Type == typeof(string))
                {
                    PlayerPrefs.SetString(t_Key, t_Value);
                }

                listOfUsedPlayerPrefEditorData[t_Index].InvokeEvent(t_Value);
            }


        }

        public static bool IsPlayerPrefKeyAlreadyInUsed(string t_Key)
        {

            foreach (PlayerPrefEditorData t_PlayerPrefEditorData in listOfUsedPlayerPrefEditorData)
            {

                if (t_Key.CompareTo(t_PlayerPrefEditorData.key) == 0)
                    return true;
            }

            return false;

        }

        public static void ResetPlayerPrefData(string t_Key)
        {

            int index = IsPlayerPrefEditorDataAlreadyInContainer(t_Key);
            if (index != -1)
            {
                listOfUsedPlayerPrefEditorData.RemoveAt(index);
                PlayerPrefs.DeleteKey(t_Key);
            }

        }

        public static void ResetAllPlayerPrefData()
        {
            foreach (PlayerPrefEditorData playerPrefEditorData in listOfUsedPlayerPrefEditorData)
            {
                PlayerPrefs.DeleteKey(playerPrefEditorData.key);
            }

            listOfUsedPlayerPrefEditorData.Clear();
        }

        #endregion

    }

#endif

    [Serializable]
    public class PlayerPrefData<T>
    {

        #region Public Variables

        public event Action<T> OnValueChangedEvent;

        #endregion

        #region Private Variables

        private CoreEnums.DataTypeForSavingData m_DataType;
        private string m_Key;

        #endregion

        #region Configuretion

        private bool AssigningDataType(T t_Value)
        {

            //Assigning :   DataType
            if (typeof(T) == typeof(bool))
            {
                m_DataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_BOOL;
                return true;
            }
            else if (typeof(T) == typeof(int))
            {
                m_DataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_INT;
                return true;
            }
            else if (typeof(T) == typeof(float))
            {
                m_DataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_FLOAT;
                return true;
            }
            else if (typeof(T) == typeof(double))
            {
                m_DataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_DOUBLE;
                return true;
            }
            else if (typeof(T) == typeof(string))
            {

                m_DataType = CoreEnums.DataTypeForSavingData.DATA_TYPE_STRING;
                return true;
            }

            CoreDebugger.Debug.LogError("Invalid Type for Value : " + t_Value);
            m_DataType = CoreEnums.DataTypeForSavingData.UNDEFINED;
            return false;
        }

        #endregion

        #region Public Callback

        public PlayerPrefData(string t_Key, T t_Value, Action<T> OnValueChanged = null)
        {
            m_Key = t_Key;

            
            if (OnValueChanged != null)
                OnValueChangedEvent += OnValueChanged;

            //if : The following key is not used, we initialized the data type and their respective value
            if (!PlayerPrefs.HasKey(t_Key))
            {
                //if : Valid DataType
                SetData(t_Value);
#if UNITY_EDITOR
                if (PlayerPrefDataSettings.IsPlayerPrefEditorDataAlreadyInContainer(t_Key) != -1)
                {
                    PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(t_Key, Convert.ChangeType(t_Value, typeof(T)).ToString(), ref OnValueChanged);
                }
                else {

                    PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(t_Key, GetData().ToString(), ref OnValueChanged);
                }
#endif
            }
            else {

                if (AssigningDataType(t_Value)) {

                    OnValueChangedEvent?.Invoke(GetData());
                }
#if UNITY_EDITOR
                PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(t_Key, GetData().ToString(), ref OnValueChanged);
#endif
            }
        }

        public string GetKey() {

            return m_Key;
        }

        public void SetData(T t_Value)
        {

            if (AssigningDataType(t_Value))
            {

                switch (m_DataType)
                {

                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_BOOL:

                        bool t_ParsedBoolValue = (bool)Convert.ChangeType(t_Value, typeof(bool));
                        PlayerPrefs.SetInt(m_Key, t_ParsedBoolValue ? 1 : 0);
                        OnValueChangedEvent?.Invoke((T)Convert.ChangeType(t_Value, typeof(bool)));

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_INT:

                        int t_ParsedIntValue = (int)Convert.ChangeType(t_Value, typeof(int));
                        PlayerPrefs.SetInt(m_Key, t_ParsedIntValue);
                        OnValueChangedEvent?.Invoke((T)Convert.ChangeType(t_Value, typeof(int)));

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_FLOAT:

                        float t_ParsedFloatValue = (float)Convert.ChangeType(t_Value, typeof(float));
                        PlayerPrefs.SetFloat(m_Key, t_ParsedFloatValue);
                        OnValueChangedEvent?.Invoke((T)Convert.ChangeType(t_Value, typeof(float)));

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_DOUBLE:

                        double t_ParsedDoubleValue = (double)Convert.ChangeType(t_Value, typeof(double));
                        PlayerPrefs.SetString(m_Key, t_ParsedDoubleValue.ToString());
                        OnValueChangedEvent?.Invoke((T)Convert.ChangeType(t_Value, typeof(double)));

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_STRING:

                        string t_ParsedStringValue = (string)Convert.ChangeType(t_Value, typeof(string));
                        PlayerPrefs.SetString(m_Key, t_ParsedStringValue);
                        OnValueChangedEvent?.Invoke((T)Convert.ChangeType(t_Value, typeof(string)));

                        break;
                }

                
            }
        }

        public T GetData()
        {

            switch (m_DataType)
            {

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_BOOL:

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(m_Key, 0) == 1 ? true : false, typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_INT:

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(m_Key, 0), typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_FLOAT:

                    return (T)Convert.ChangeType(PlayerPrefs.GetFloat(m_Key, 0), typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_DOUBLE:

                    return (T)Convert.ChangeType(PlayerPrefs.GetString(m_Key, "0"), typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_STRING:

                    return (T)Convert.ChangeType(PlayerPrefs.GetString(m_Key, ""), typeof(T));

            }

            return (T)Convert.ChangeType(PlayerPrefs.GetInt(m_Key, 0), typeof(T));
        }

        #endregion


    }

}

