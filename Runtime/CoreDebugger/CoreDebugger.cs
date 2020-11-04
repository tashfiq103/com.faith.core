namespace com.faith.core
{
    using UnityEngine;

    public static class CoreDebugger
    {
        public static class Debug{

            private const string _debugMessagePrefix = "[CoreDebug]: ";

            //Section   :   LogWarning  :   Verbose
            //------------------------
            public static void LogWarning(object message, Color color = new Color())
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}",
                        color == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        message,
                        color == Color.black ? "" : "</color>"));
            }

            public static void LogWarning(object message, GameConfiguratorAsset configuratorAsset)
            {
                if (configuratorAsset.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForWarning == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForWarning) + ">"),
                        _debugMessagePrefix,
                        message,
                        configuratorAsset.colorForWarning == Color.black ? "" : "</color>"));
            }

            public static void LogWarning(object message, Object context, Color color = new Color())
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}",
                        color == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        message,
                        color == Color.black ? "" : "</color>"),
                        context);
            }

            public static void LogWarning(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {
                if (configuratorAsset.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForWarning == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForWarning) + ">"),
                        _debugMessagePrefix,
                        message,
                        configuratorAsset.colorForWarning == Color.black ? "" : "</color>"),
                        context);
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
            public static void Log(object message, Color color = new Color()) {

                if(GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}",
                        color == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        message,
                        color == Color.black ? "" : "</color>"));
            }

            public static void Log(object message, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForLog == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLog) + ">"),
                        _debugMessagePrefix,
                        message,
                        configuratorAsset.colorForLog == Color.black ? "" : "</color>"));
            }

            public static void Log(object message, Object context, Color color = new Color())
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}",
                        color == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        message,
                        color == Color.black ? "" : "</color>"),
                        context);
            }

            public static void Log(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForLog == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLog) + ">"),
                        _debugMessagePrefix,
                        message,
                        configuratorAsset.colorForLog == Color.black ? "" : "</color>"),
                        context);
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
            public static void LogError(object message, Color color = new Color())
            {

                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        color == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        message,
                        color == Color.black ? "" : "</color>"));
            }

            public static void LogError(object message, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info || configuratorAsset.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForLogError == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLogError) + ">"),
                        _debugMessagePrefix,
                        message,
                        configuratorAsset.colorForLogError == Color.black ? "" : "</color>"));
            }

            public static void LogError(object message, Object context, Color color = new Color())
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        color == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        message,
                        color == Color.black ? "" : "</color>"),
                        context);
            }

            public static void LogError(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info || configuratorAsset.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForLogError == Color.black ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLogError) + ">"),
                        _debugMessagePrefix,
                        message,
                        configuratorAsset.colorForLogError == Color.black ? "" : "</color>"),
                        context);
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


