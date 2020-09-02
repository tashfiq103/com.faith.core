namespace com.faith.core
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class PlayerPrefDataSettings
    {


        #region Custom DataType

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

        public static List<string> listOfUsedPlayerPrefKey = new List<string>();

        #endregion

        #region Public Callback

        public static bool IsPlayerPrefKeyAlreadyInUsed(string t_Key)
        {

            if (!listOfUsedPlayerPrefKey.Contains(t_Key))
                return false;

            //Debug.LogError("The Following Key : " + t_Key + ", is already used as PlayerPrefs");

            return true;
        }

        public static void EnlistUniqueKeyAsUsedPlayerPrefs(string t_Key)
        {
            if(!listOfUsedPlayerPrefKey.Contains(t_Key))
                listOfUsedPlayerPrefKey.Add(t_Key);
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

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_INT:

                        int t_ParsedIntValue = (int)Convert.ChangeType(t_Value, typeof(int));
                        PlayerPrefs.SetInt(m_Key, t_ParsedIntValue);

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_FLOAT:

                        float t_ParsedFloatValue = (float)Convert.ChangeType(t_Value, typeof(float));
                        PlayerPrefs.SetFloat(m_Key, t_ParsedFloatValue);

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_DOUBLE:

                        double t_ParsedDoubleValue = (double)Convert.ChangeType(t_Value, typeof(double));
                        PlayerPrefs.SetString(m_Key, t_ParsedDoubleValue.ToString());

                        break;
                    case PlayerPrefDataSettings.DataTypeForPlayerPref.DATA_TYPE_STRING:

                        string t_ParsedStringValue = (string)Convert.ChangeType(t_Value, typeof(string));
                        PlayerPrefs.SetString(m_Key, t_ParsedStringValue);

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

