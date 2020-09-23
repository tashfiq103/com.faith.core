namespace com.faith.core
{
    using UnityEngine;

    public static class CoreDebugger
    {
        public static class Debug{

            //Section   :   LogWarning  :   Verbose
            //------------------------
            public static void LogWarning(object message)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(message);
            }

            public static void LogWarning(object message, Object context)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(message, context);
            }

            public static void LogWarningFormat(string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarningFormat(message, args);
            }

            public static void LogWarningFormat(Object context, string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarningFormat(context, message, args);
            }


            //Section   :   Log     :   Verbose|Info
            //------------------------
            public static void Log(object message) {

                if(GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(message);
            }

            public static void Log(object message, Object context)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(message, context);
            }

            public static void LogFormat(string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.LogFormat(message,args);
            }

            public static void LogFormat(Object context, string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.LogFormat(context, message, args);
            }

            //Section   :   LogError    :   Verbose|Info|Error
            //------------------------
            public static void LogError(object message)
            {

                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(message);
            }

            public static void LogError(object message, Object context)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(message, context);
            }

            public static void LogErrorFormat(string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogErrorFormat(message, args);
            }

            public static void LogErrorFormat(Object context, string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogErrorFormat(context, message, args);
            }
        }
    }
}


