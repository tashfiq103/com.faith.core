namespace com.faith.core
{
    using System;

    public abstract class BaseDataClass<T>
    {
        #region Private Variables

        protected string _key;
        protected CoreEnums.DataTypeForSavingData _dataType;
        protected event Action<T> OnValueChangedEvent;

        #endregion

        #region Configuretion

        protected bool AssigningDataType(T t_Value)
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

            CoreDebugger.Debug.LogError("Invalid DataType for Value : " + t_Value);
            _dataType = CoreEnums.DataTypeForSavingData.UNDEFINED;
            return false;
        }

        #endregion

        #region Public Callback

        public string GetKey()
        {
            return _key;
        }


        public void InvokeOnValueChangedEvent(T value) {

            OnValueChangedEvent?.Invoke(value);
        }

        #endregion
    }
}


