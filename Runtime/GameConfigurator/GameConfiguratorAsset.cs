namespace com.faith.core
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public class GameConfiguratorAsset : ScriptableObject
    {

        #region SerializedField

        [SerializeField] private bool _isUsedByCentralGameConfiguretion = false;
        [SerializeField] private bool _linkWithCentralGameConfiguretion = false;

        [SerializeField] private bool _enableStackTrace;
        [SerializeField,Range(10,999)] private int _numberOfLog = 100;
        [SerializeField] private LogType _clearLogType;
        [SerializeField] private List<CoreDebugger.Debug.DebugInfo> _listOfLogInfo = new List<CoreDebugger.Debug.DebugInfo>();

        [SerializeField] private CoreEnums.GameMode _gameMode = CoreEnums.GameMode.DEBUG;
        [SerializeField] private CoreEnums.LogType _logType = CoreEnums.LogType.Verbose;
        [SerializeField] private CoreEnums.DataSavingMode _dataSavingMode = CoreEnums.DataSavingMode.PlayerPrefsData;

        #endregion

        #region Public Variables 

#if UNITY_EDITOR
        public bool EditorAccessIfUsedByCentralGameConfiguretion {
            get
            {
                return _isUsedByCentralGameConfiguretion;
            }
        }
        public bool EditorAccessIfLinkWithCentralGameConfiguretion
        {
            get
            {
                return _linkWithCentralGameConfiguretion;
            }
        }

        public List<CoreDebugger.Debug.DebugInfo> EditorListOfLogInfo { get { return _listOfLogInfo; } }

#endif

        public CoreEnums.GameMode gameMode { get { return _linkWithCentralGameConfiguretion ? GameConfiguratorManager.gameMode : _gameMode; } }

        public CoreEnums.LogType logType { get { return _linkWithCentralGameConfiguretion ? GameConfiguratorManager.logType : _logType; } }
        public string prefix = "";
        public Color colorForLog = new Color();
        public Color colorForWarning = Color.yellow;
        public Color colorForLogError = Color.red;

        public CoreEnums.DataSavingMode dataSavingMode { get { return _linkWithCentralGameConfiguretion ? GameConfiguratorManager.dataSavingMode : _dataSavingMode; } }
        public bool dataSaveWhenSceneUnloaded = true;
        public bool dataSaveWhenApplicationLoseFocus = true;
        public bool dataSaveWhenApplicationQuit = true;
        [Range(1,60)]
        public float snapshotFrequenceyInSec = 15;

        #endregion

        #region ScriptableObject

        private void OnEnable()
        {
            if (_enableStackTrace) {

                if (_listOfLogInfo == null) _listOfLogInfo = new List<CoreDebugger.Debug.DebugInfo>();

                Application.logMessageReceivedThreaded += LogMessageReciever;
            }
        }

        private void OnDisable()
        {
            if (_enableStackTrace)
            {
                Application.logMessageReceivedThreaded -= LogMessageReciever;
            }
        }

        #endregion

        #region Configuretion

        private void LogMessageReciever(string condition, string stackTrace, LogType logType)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrWhiteSpace(prefix))
            {
                Debug.LogWarning(string.Format("No prefix was found for [CoreDebugger]_['{0}']. Assigning it's name as new prefix = {1}", prefix, name));
                prefix = name;
                return;
            }

            string filter = string.Format("{0}_[{1}]", CoreDebugger.Debug._debugMessagePrefix, prefix);
            if (condition.Contains(filter))
            {

                if (_listOfLogInfo.Count >= _numberOfLog)
                    _listOfLogInfo.RemoveAt(0);

                _listOfLogInfo.Add(new CoreDebugger.Debug.DebugInfo()
                {
                    timeStamp = DateTime.Now.ToString(),
                    condition = condition,
                    stackTrace = stackTrace,
                    logType = logType
                });
            }
        }

        #endregion

        #region Public Callback

        public void ClearAllLog()
        {
            _listOfLogInfo.Clear();
        }

        public void ClearLog(LogType logType) {

            List<int> listOfRemovingIndex   = new List<int>();
            int numberOfLog                 = _listOfLogInfo.Count;

            for (int i = 0; i < numberOfLog; i++) {
                if (_listOfLogInfo[i].logType == logType)
                    listOfRemovingIndex.Add(i);
            }

            int leftShiftValue = 0;
            foreach (int index in listOfRemovingIndex) {
                _listOfLogInfo.RemoveAt(index- leftShiftValue);
                leftShiftValue++;
            }
        }

        public int NumberOfLog { get { return _listOfLogInfo.Count; } }

        public int GetNumberOfLog(LogType logType) {

            int numberOfLog = 0;
            foreach (CoreDebugger.Debug.DebugInfo debugInfo in _listOfLogInfo) {

                if (logType == debugInfo.logType)
                    numberOfLog++;
            }

            return numberOfLog;
        }

        #endregion

    }
}

