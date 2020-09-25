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

            public void AssignValueChangedEvent<T>(string value, ref Action<T> OnValueChanged) {

                this.value = value;

                if (typeof(T) == typeof(bool))
                {
                    OnValueChangedForBool += (Action<bool>)Convert.ChangeType(OnValueChanged, typeof(Action<bool>));
                }
                else if (typeof(T) == typeof(int)){

                    OnValueChangedForInt += (Action<int>)Convert.ChangeType(OnValueChanged, typeof(Action<int>));
                }
                else if (typeof(T) == typeof(float)){

                    OnValueChangedForFloat += (Action<float>)Convert.ChangeType(OnValueChanged, typeof(Action<float>));
                }
                else if (typeof(T) == typeof(double)){

                    OnValueChangedForDouble += (Action<double>)Convert.ChangeType(OnValueChanged, typeof(Action<double>));
                }
                else if (typeof(T) == typeof(string)){

                    OnValueChangedForString += (Action<string>)Convert.ChangeType(OnValueChanged, typeof(Action<string>));
                }

                InvokeEvent(this.value); ;
            }

            public T GetData<T>()
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            public void InvokeEvent(string value) {

                this.value = value;

                if (type == typeof(bool)) {

                    OnValueChangedForBool?.Invoke(GetData<bool>());
                }
                else if (type == typeof(int)){

                    OnValueChangedForInt?.Invoke(GetData<int>());
                }
                else if (type == typeof(float))
                {
                    OnValueChangedForFloat?.Invoke(GetData<float>());
                }
                else if (type == typeof(double))
                {
                    OnValueChangedForDouble?.Invoke(GetData<double>());
                }
                else if (type == typeof(string))
                {
                    OnValueChangedForString?.Invoke(GetData<string>());
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

        public static void EnlistPlayerPrefEditorDataInContainer<T>(string key, string value, ref Action<T> OnValueChanged)
        {

            int t_Result = IsPlayerPrefEditorDataAlreadyInContainer(key);
            if (t_Result == -1)
            {
                PlayerPrefEditorData playerPrefEditorData = new PlayerPrefEditorData()
                {
                    key = key,
                    type = typeof(T),
                    value = value
                };
                playerPrefEditorData.AssignValueChangedEvent(value, ref OnValueChanged);

                listOfUsedPlayerPrefEditorData.Add(playerPrefEditorData);
            }
            else
            {

                listOfUsedPlayerPrefEditorData[t_Result].type = typeof(T);
                listOfUsedPlayerPrefEditorData[t_Result].value = value;
            }


        }

        public static void RegisterOnValueChangedEvent<T>(string key, ref Action<T> OnValueChanged) {

            int index = IsPlayerPrefEditorDataAlreadyInContainer(key);
            if (index != -1) {

                listOfUsedPlayerPrefEditorData[index].AssignValueChangedEvent(GetData<T>(key).ToString(), ref OnValueChanged);
            }
        }

        public static void SetData<T>(string t_Key, string t_Value)
        {
            int t_Index = IsPlayerPrefEditorDataAlreadyInContainer(t_Key);
            if (t_Index != -1)
            {

                if (typeof(T) == typeof(bool))
                {
                    PlayerPrefs.SetInt(t_Key, string.Compare(t_Value, "False") == 0 ? 0 : 1);
                }
                else if (typeof(T) == typeof(int))
                {
                    PlayerPrefs.SetInt(t_Key, (int)Convert.ChangeType(t_Value, typeof(int)));
                }
                else if (typeof(T) == typeof(float))
                {
                    PlayerPrefs.SetFloat(t_Key, (float)Convert.ChangeType(t_Value, typeof(float)));
                }
                else if (typeof(T) == typeof(double))
                {
                    PlayerPrefs.SetString(t_Key, ((double)Convert.ChangeType(t_Value, typeof(double))).ToString());
                }
                else if (typeof(T) == typeof(string))
                {
                    PlayerPrefs.SetString(t_Key, t_Value);
                }

                listOfUsedPlayerPrefEditorData[t_Index].InvokeEvent(t_Value);
            }
        }

        public static T GetData<T>(string key) {

            int index = IsPlayerPrefEditorDataAlreadyInContainer(key);
            if (index != -1) {

                if (typeof(T) == typeof(bool)) {

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(key, 0) == 1 ? true : false, typeof(T));
                }
                else if (typeof(T) == typeof(int))
                {

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(key, 0), typeof(T));
                }
                else if (typeof(T) == typeof(float))
                {

                    return (T)Convert.ChangeType(PlayerPrefs.GetFloat(key, 0), typeof(T));
                }
                else if (typeof(T) == typeof(double))
                {

                    return (T)Convert.ChangeType(PlayerPrefs.GetString(key, "0"), typeof(T));
                }
                else if (typeof(T) == typeof(bool))
                {

                    return (T)Convert.ChangeType(PlayerPrefs.GetString(key, ""), typeof(T));
                }
            }

            return (T)Convert.ChangeType(PlayerPrefs.GetInt(key, 0), typeof(T));
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
    public class PlayerPrefData<T>  :   BaseDataClass<T>
    {

        #region Public Callback

        public PlayerPrefData(string t_Key, T t_Value, Action<T> OnValueChanged = null)
        {
            _key = t_Key;


            RegisterOnValueChangedEvent(OnValueChanged);

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
                    InvokeOnValueChangedEvent(GetData());
                }
#if UNITY_EDITOR
                PlayerPrefDataSettings.EnlistPlayerPrefEditorDataInContainer(t_Key, GetData().ToString(), ref OnValueChanged);
#endif
            }
        }

        public void RegisterOnValueChangedEvent(Action<T> OnValueChanged) {

            if (OnValueChanged != null) {

                OnValueChangedEvent += OnValueChanged;
                InvokeOnValueChangedEvent(GetData());
#if UNITY_EDITOR
                PlayerPrefDataSettings.RegisterOnValueChangedEvent(_key ,ref OnValueChanged);
#endif
            }
        }

        public void SetData(T value)
        {

            if (AssigningDataType(value))
            {

                int index = PlayerPrefDataSettings.IsPlayerPrefEditorDataAlreadyInContainer(_key);

                switch (_dataType)
                {

                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_BOOL:

                        bool t_ParsedBoolValue = (bool)Convert.ChangeType(value, typeof(bool));
                        PlayerPrefs.SetInt(_key, t_ParsedBoolValue ? 1 : 0);
                        InvokeOnValueChangedEvent((T)Convert.ChangeType(value, typeof(bool)));

#if UNITY_EDITOR
                        if (index != -1) PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData[index].InvokeEvent(t_ParsedBoolValue.ToString());
#endif

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_INT:

                        int t_ParsedIntValue = (int)Convert.ChangeType(value, typeof(int));
                        PlayerPrefs.SetInt(_key, t_ParsedIntValue);
                        InvokeOnValueChangedEvent((T)Convert.ChangeType(value, typeof(int)));

#if UNITY_EDITOR
                        if (index != -1) PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData[index].InvokeEvent(t_ParsedIntValue.ToString());
#endif

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_FLOAT:

                        float t_ParsedFloatValue = (float)Convert.ChangeType(value, typeof(float));
                        PlayerPrefs.SetFloat(_key, t_ParsedFloatValue);
                        InvokeOnValueChangedEvent((T)Convert.ChangeType(value, typeof(float)));

#if UNITY_EDITOR
                        if (index != -1) PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData[index].InvokeEvent(t_ParsedFloatValue.ToString());
#endif

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_DOUBLE:

                        double t_ParsedDoubleValue = (double)Convert.ChangeType(value, typeof(double));
                        PlayerPrefs.SetString(_key, t_ParsedDoubleValue.ToString());
                        InvokeOnValueChangedEvent((T)Convert.ChangeType(value, typeof(double)));

#if UNITY_EDITOR
                        if (index != -1) PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData[index].InvokeEvent(t_ParsedDoubleValue.ToString());
#endif

                        break;
                    case CoreEnums.DataTypeForSavingData.DATA_TYPE_STRING:

                        string t_ParsedStringValue = (string)Convert.ChangeType(value, typeof(string));
                        PlayerPrefs.SetString(_key, t_ParsedStringValue);
                        InvokeOnValueChangedEvent((T)Convert.ChangeType(value, typeof(string)));

#if UNITY_EDITOR
                        if (index != -1) PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData[index].InvokeEvent(t_ParsedStringValue.ToString());
#endif

                        break;
                }
            }
        }

        public T GetData()
        {

            switch (_dataType)
            {

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_BOOL:

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(_key, 0) == 1 ? true : false, typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_INT:

                    return (T)Convert.ChangeType(PlayerPrefs.GetInt(_key, 0), typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_FLOAT:

                    return (T)Convert.ChangeType(PlayerPrefs.GetFloat(_key, 0), typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_DOUBLE:

                    return (T)Convert.ChangeType(PlayerPrefs.GetString(_key, "0"), typeof(T));

                case CoreEnums.DataTypeForSavingData.DATA_TYPE_STRING:

                    return (T)Convert.ChangeType(PlayerPrefs.GetString(_key, ""), typeof(T));

            }

            return (T)Convert.ChangeType(PlayerPrefs.GetInt(_key, 0), typeof(T));
        }

#endregion
    }

}

