namespace com.faith.core
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class PlayerPrefDataSettings
    {
        #region Custom DataType

#if UNITY_EDITOR

        [Serializable]
        public class PlayerPrefEditorData
        {
            public string key;
            public Type type;
            public string value;
        }
#endif

        public enum DataTypeForPlayerPref
        {
            DATA_TYPE_BOOL,
            DATA_TYPE_INT,
            DATA_TYPE_FLOAT,
            DATA_TYPE_DOUBLE,
            DATA_TYPE_STRING,
            UNDEFINED
        }


        #endregion

        #region Private Variables

#if UNITY_EDITOR
        [SerializeField]
        public static List<PlayerPrefEditorData> listOfUsedPlayerPrefEditorData    = new List<PlayerPrefEditorData>();
#endif

        public static List<string> listOfUsedPlayerPrefKey = new List<string>();

        #endregion

        #region Public Callback

#if UNITY_EDITOR

        public static int IsPlayerPrefEditorDataAlreadyInContainer(string t_Key) {

            int t_NumberOfEnlistedPlayerPrefEditorData = listOfUsedPlayerPrefEditorData.Count;
            for (int i = 0; i < t_NumberOfEnlistedPlayerPrefEditorData; i++) {

                if (string.Equals(t_Key, listOfUsedPlayerPrefEditorData[i].key))
                    return i;
            }

            return -1;
        }

        public static void EnlistPlayerPrefEditorDataInContainer(string t_Key, Type t_Type, string t_Value) {

            int t_Result = IsPlayerPrefEditorDataAlreadyInContainer(t_Key);
            if (t_Result == -1)
            {

                listOfUsedPlayerPrefEditorData.Add(new PlayerPrefEditorData()
                {
                    key = t_Key,
                    type = t_Type,
                    value = t_Value
                });
            }
            else {

                listOfUsedPlayerPrefEditorData[t_Result].type   = t_Type;
                listOfUsedPlayerPrefEditorData[t_Result].value  = t_Value;
            }

            
        }

        public static void SetData(string t_Key, Type t_Type, string t_Value)
        {
            int t_Index = IsPlayerPrefEditorDataAlreadyInContainer(t_Key);
            if (t_Index != -1) {

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

                listOfUsedPlayerPrefEditorData[t_Index].value = t_Value;
            }

            
        }

#endif

        public static bool IsPlayerPrefKeyAlreadyInUsed(string t_Key)
        {

            if (!listOfUsedPlayerPrefKey.Contains(t_Key))
                return false;

            //Debug.LogError("The Following Key : " + t_Key + ", is already used as PlayerPrefs");

            return true;
        }

        public static void EnlistUniqueKeyAsUsedPlayerPrefs(string t_Key)
        {
            if (!IsPlayerPrefKeyAlreadyInUsed(t_Key))
            {
                listOfUsedPlayerPrefKey.Add(t_Key);
            }
        }

        

        public static void ResetPlayerPrefData(string t_Key)
        {

            if (IsPlayerPrefKeyAlreadyInUsed(t_Key))
            {

                PlayerPrefs.DeleteKey(t_Key);
                listOfUsedPlayerPrefKey.Remove(t_Key);
            }

        }

        public static void ResetAllPlayerPrefData()
        {
            foreach (string t_Key in listOfUsedPlayerPrefKey)
            {
                PlayerPrefs.DeleteKey(t_Key);
            }

            listOfUsedPlayerPrefKey.Clear();
        }

        #endregion

    }

    [Serializable]
    public class PlayerPrefData<T>
    {

        #region Private Variables

        private PlayerPrefDataSettings.DataTypeForPlayerPref m_DataType;
        private string m_Key;

        #endregion

        #region Configuretion

        private bool AssigningDataType(T t_Value)
        {

            //Assigning :   DataType
            if (typeof(T) == typeof(bool))
            {
                m_DataType = PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_BOOL;
                return true;
            }
            else if (typeof(T) == typeof(int))
            {
                m_DataType = PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_INT;
                return true;
            }
            else if (typeof(T) == typeof(float))
            {
                m_DataType = PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_FLOAT;
                return true;
            }
            else if (typeof(T) == typeof(double))
            {
                m_DataType = PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_DOUBLE;
                return true;
            }
            else if (typeof(T) == typeof(string))
            {

                m_DataType = PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_STRING;
                return true;
            }

            Debug.LogError("Invalid Type for Value : " + t_Value);
            m_DataType = PlayerPrefDataSettings.DataTypeForPlayerPref.UNDEFINED;
            return false;
        }

        #endregion

        #region Public Callback

        public PlayerPrefData(string t_Key, T t_Value)
        {
            m_Key = t_Key;

            PlayerPrefDataSettings.EnlistUniqueKeyAsUsedPlayerPrefs(t_Key);

            //if : The following key is not used, we initialized the data type and their respective value
            if (!PlayerPrefs.HasKey(t_Key))
            {
                //if : Valid DataType
                if (AssigningDataType(t_Value))
                {
                    SetData(t_Value);
                }
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

                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_BOOL:

                        bool t_ParsedBoolValue = (bool)Convert.ChangeType(t_Value, typeof(bool));
                        PlayerPrefs.SetInt(m_Key, t_ParsedBoolValue ? 1 : 0);

#if UNITY_EDITOR
                        PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(m_Key, typeof(bool), t_ParsedBoolValue.ToString());
#endif

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_INT:

                        int t_ParsedIntValue = (int)Convert.ChangeType(t_Value, typeof(int));
                        PlayerPrefs.SetInt(m_Key, t_ParsedIntValue);

#if UNITY_EDITOR
                        PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(m_Key, typeof(int), t_ParsedIntValue.ToString());
#endif

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_FLOAT:

                        float t_ParsedFloatValue = (float)Convert.ChangeType(t_Value, typeof(float));
                        PlayerPrefs.SetFloat(m_Key, t_ParsedFloatValue);

#if UNITY_EDITOR
                        PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(m_Key, typeof(float), t_ParsedFloatValue.ToString());
#endif

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_DOUBLE:

                        double t_ParsedDoubleValue = (double)Convert.ChangeType(t_Value, typeof(double));
                        PlayerPrefs.SetString(m_Key, t_ParsedDoubleValue.ToString());

#if UNITY_EDITOR
                        PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(m_Key, typeof(double), t_ParsedDoubleValue.ToString());
#endif

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_STRING:

                        string t_ParsedStringValue = (string)Convert.ChangeType(t_Value, typeof(string));
                        PlayerPrefs.SetString(m_Key, t_ParsedStringValue);

#if UNITY_EDITOR
                        PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(m_Key, typeof(string), t_ParsedStringValue.ToString());
#endif

                        break;
                }
            }
        }

        public T GetData()
        {

            switch (m_DataType)
            {

                case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_BOOL:

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(m_Key, 0) == 1 ? true : false, typeof(T));

                case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_INT:

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(m_Key, 0), typeof(T));

                case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_FLOAT:

                    return (T)Convert.ChangeType(PlayerPrefs.GetFloat(m_Key, 0), typeof(T));

                case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_DOUBLE:

                    return (T)Convert.ChangeType(PlayerPrefs.GetFloat(m_Key, 0), typeof(T));

                case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_STRING:

                    return (T)Convert.ChangeType(PlayerPrefs.GetString(m_Key, ""), typeof(T));

            }

            Debug.Log("GetValueFromHere");
            return (T)Convert.ChangeType(PlayerPrefs.GetInt(m_Key, 0), typeof(T));
        }

        #endregion


    }

}

