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

        public enum AccountBalanceUpdateState
        {
            ADDED,
            DEDUCTED,
            NONE
        }
    }
}

