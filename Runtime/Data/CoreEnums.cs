namespace com.faith.core
{
    public class CoreEnums
    {

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
    }
}

