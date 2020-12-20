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
                public string timeStamp;
                public string condition;
                public string stackTrace;
                public LogType logType;
            }

            #region Section   :   LogWarning  :   Verbose

            public static void LogWarning(object message, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                {
                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }


                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        hexColorPostfix));
                }
            }

            public static void LogWarning(object message, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose)
                {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    Color color = configuratorAsset.colorForWarning;
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        hexColorPostfix));
                }
            }

            public static void LogWarning(object message, Object context, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose)
                {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        hexColorPostfix),
                        context);
                }

            }

            public static void LogWarning(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {
                if (configuratorAsset.logType == CoreEnums.LogType.Verbose)
                {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    Color color = configuratorAsset.colorForWarning;
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.LogWarning(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        hexColorPostfix),
                        context);
                }

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


            #endregion


            //-----------------
            #region Section   :   Log     :   Verbose|Info

            public static void Log(object message, Color color = new Color(), string prefix = "")
            {

                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info) {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        hexColorPostfix));
                }
                    
            }

            public static void Log(object message, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info) {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    Color color = configuratorAsset.colorForLog;
                    if (color != new Color())
                    {
                        color.a = 1;
                        Debug.Log("ColorValue : " + color);
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        hexColorPostfix));
                }
                    
            }

            public static void Log(object message, Object context, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info) {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        hexColorPostfix),
                        context);
                }
                    
            }

            public static void Log(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info) {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    Color color = configuratorAsset.colorForLog;
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.Log(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        hexColorPostfix),
                        context);
                }
            }

            public static void LogFormat(string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.LogFormat(message, args);
            }

            public static void LogFormat(Object context, string message, params object[] args)
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info)
                    UnityEngine.Debug.LogFormat(context, message, args);
            }


            #endregion


            //-----------------
            #region Section   :   LogError    :   Verbose|Info|Error

            public static void LogError(object message, Color color = new Color(), string prefix = "")
            {

                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error) {

                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        hexColorPostfix));
                }
                    
            }

            public static void LogError(object message, GameConfiguratorAsset configuratorAsset)
            {
                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info || configuratorAsset.logType == CoreEnums.LogType.Error)
                {
                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    Color color = configuratorAsset.colorForLogError;
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        hexColorPostfix));
                } 
            }

            public static void LogError(object message, Object context, Color color = new Color(), string prefix = "")
            {
                if (GameConfiguratorManager.logType == CoreEnums.LogType.Verbose || GameConfiguratorManager.logType == CoreEnums.LogType.Info || GameConfiguratorManager.logType == CoreEnums.LogType.Error)
                {
                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        prefix == "" ? ": " : "_[" + prefix + "]: ",
                        message,
                        hexColorPostfix),
                        context);
                }
                    
            }

            public static void LogError(object message, Object context, GameConfiguratorAsset configuratorAsset)
            {

                if (configuratorAsset.logType == CoreEnums.LogType.Verbose || configuratorAsset.logType == CoreEnums.LogType.Info || configuratorAsset.logType == CoreEnums.LogType.Error)
                {
                    string hexColorPrefix = "";
                    string hexColorPostfix = "";
                    Color color = configuratorAsset.colorForLogError;
                    if (color != new Color())
                    {
                        color.a = 1;
                        hexColorPrefix = string.Format("<color={0}>", StringOperation.GetHexColorFromRGBColor(color));
                        hexColorPostfix = "</color>";
                    }

                    UnityEngine.Debug.LogError(string.Format("{0}{1}{2}{3}{4}",
                        hexColorPrefix,
                        _debugMessagePrefix,
                        configuratorAsset.prefix == "" ? ": " : "_[" + configuratorAsset.prefix + "]: ",
                        message,
                        hexColorPostfix),
                        context);
                }
                    
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



            #endregion

        }
    }
}


