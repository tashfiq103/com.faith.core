namespace com.faith.core
{
    public class CoreEnums
    {

        public enum CorePackageStatus
        {
            InDevelopment,
            Production
        }

        public enum GameMode
        {
            DEBUG,
            PRODUCTION
        }

        public enum LogType
        {
            Verbose,
            Info,
            Error,
            None
        }

        public enum DataTypeForSavingData
        {
            DATA_TYPE_BOOL,
            DATA_TYPE_INT,
            DATA_TYPE_FLOAT,
            DATA_TYPE_DOUBLE,
            DATA_TYPE_STRING,
            UNDEFINED
        }

        public enum DataSavingMode
        {
            BinaryFormater,
            PlayerPrefsData
        }

        public enum InstanceBehaviour
        {
            UseAsReference,
            CashedAsInstance,
            Singleton
        }

        public enum ValueChangedState
        {
            VALUE_INCREASED,
            VALUE_DECREASED,
            VALUE_UNCHANGED,
            VALUE_UNDEFINED
        }
    }
}

