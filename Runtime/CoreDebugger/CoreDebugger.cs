namespace com.faith.core
{
    using UnityEngine;
    public static class CoreDebugger
    {
        

        public static class Debug{

            public const string _debugMessagePrefix = "[CoreDebug]";

            [System.Serializable]
            public class DebugInfo
            {
                public System.DateTime timeStamp;
                public string condition;
                public string stackTrace;
                public LogType logType;
            }

            //Section   :   LogWarning  :   Verbose
            //------------------------
            public static void LogWarning(object message, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        color == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        color == new Color() ? "" : "</color>"));
            }

            public static void LogWarning(object message, GameConfiguratorAsset configuratorAsset)
            {
                if (configuratorAsset.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        configuratorAsset.colorForWarning == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForWarning) + ">"),
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        configuratorAsset.colorForWarning == new Color() ? "" : "</color>"));
            }

            public static void LogWarning(object message, Object context, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        color == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        color == new Color() ? "" : "</color>"),
                        context);
            }

            public static void LogWarning(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {
                if (configuratorAsset.logType == CoreEnums.LogType.Verbose)
                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        configuratorAsset.colorForWarning == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForWarning) + ">"),
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        configuratorAsset.colorForWarning == new Color() ? "" : "</color>"),
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
            public static void Log(object message, Color color = new Color(), string prefix = "") {

                if(GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        color == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        color == new Color() ? "" : "</color>"));
            }

            public static void Log(object message, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        configuratorAsset.colorForLog == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLog) + ">"),
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        configuratorAsset.colorForLog == new Color() ? "" : "</color>"));
            }

            public static void Log(object message, Object context, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        color == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        color == new Color() ? "" : "</color>"),
                        context);
            }

            public static void Log(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        configuratorAsset.colorForLog == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLog) + ">"),
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        configuratorAsset.colorForLog == new Color() ? "" : "</color>"),
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
            public static void LogError(object message, Color color = new Color(), string prefix = "")
            {

                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        color == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        color == new Color() ? "" : "</color>"));
            }

            public static void LogError(object message, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info || configuratorAsset.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForLogError == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLogError) + ">"),
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        configuratorAsset.colorForLogError == new Color() ? "" : "</color>"));
            }

            public static void LogError(object message, Object context, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        color == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(color) + ">"),
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        color == new Color() ? "" : "</color>"),
                        context);
            }

            public static void LogError(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info || configuratorAsset.logType == CoreEnums.LogType.Error)
                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}",
                        configuratorAsset.colorForLogError == new Color() ? "" : ("<color=" + StringOperation.GetHexColorFromRGBColor(configuratorAsset.colorForLogError) + ">"),
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        configuratorAsset.colorForLogError == new Color() ? "" : "</color>"),
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


