namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using System.Collections.Generic;
    

    public class CoreConsole : BaseEditorWindowClass, IPreprocessBuildWithReport
    {
        #region Custom Variables

        private class ConsoleDebugInfo
        {
            public GameConfiguratorAsset        gameConfig;
            public CoreDebugger.Debug.DebugInfo debugInfo;
        }

        #endregion

        #region Public Variables

        public int callbackOrder => throw new System.NotImplementedException();

        #endregion

        #region Private Variables

        private static List<CoreConsole> _listOfCoreConsole = new List<CoreConsole>();
        private static List<GameConfiguratorAsset> _listOfGameConfiguretorAsset = new List<GameConfiguratorAsset>();

        private CoreConsole _editorWindowOfCoreConsole;

        private GUIContent _GUIContentForClearDropdownButton= new GUIContent();

        private GUIContent _GUIContentForTogglingInfoLog = new GUIContent();
        private GUIContent _GUIContentForTogglingWarningLog = new GUIContent();
        private GUIContent _GUIContentForTogglingErrorLog = new GUIContent();

        private GUIContent _GUIContentForSelectedConfigAsset = new GUIContent();

        private GUIContent _GUIContentForInfoLog = new GUIContent();
        private GUIContent _GUIContentForWarningLog = new GUIContent();
        private GUIContent _GUIContentForErrorLog = new GUIContent();

        private GUIContent _GUIContentForStackTrace = new GUIContent();

        private bool _isClearOnEnteringPlayMode { get { return _clearOptionStatus[0]; } }
        private bool _isClearnOnBuild { get { return _clearOptionStatus[1]; } }

        private bool _errorPause = true;
        private bool _showTimeStamp = false;

        private bool _enableInfoLog = true;
        private bool _enableLogWarning = true;
        private bool _enableLogError = true;

        private float _contentHeightForLogsInList = 30;

        private bool[]      _clearOptionStatus= new bool[] { true, false };
        private string[]    _clearOptionLable = new string[] { "Clear on Play", "Clear on Build" };

        private int _selectedLogIndex;
        private string _selectedLogCondition;
        private string _selectedLogStackTrace;
        private Vector2 _scrollPositionForListOfLog;
        private Vector2 _scrollPositionForLogMessage;

        private Color _selectedLogColor;
        private Color defaultBackgroundColor;
        private Color defaultContentColor;

        private List<bool> _gameConfiguretorEnableStatus;
        private List<string> _gameConfiguretorOptionLabels;

        #endregion

        #region Editor  :   Static

        [MenuItem("FAITH/Core/Core Console", priority = 3)]
        public static void ShowWindow() {

            if (_listOfCoreConsole == null)
                _listOfCoreConsole = new List<CoreConsole>();

            CoreConsole newCoreConsole = CreateInstance<CoreConsole>();
            newCoreConsole.CreateCoreConsole();
        }

        

        #endregion

        #region Editor, Interface

        public override void OnEnable()
        {
            base.OnEnable();

            UpdateGameConfiguretorAsset();
        }

        public void OnGUI()
        {
            HeaderGUI();

            DrawLogListGUI();

            DrawLogMessageGUI();
        }

        public void OnDestroy()
        {
            _listOfCoreConsole.Remove(this);
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if(_isClearnOnBuild)
                ClearAllLog();
        }

        #endregion

        #region Configuretion

        

        private bool IsSelectedLog(int logIndex) {

            if (_selectedLogIndex != -1 && _selectedLogIndex == logIndex)
                return true;

            return false;
        }

        private int GetNumberOfLog(LogType logType)
        {

            int result = 0;
            int numberOfLogType = _listOfGameConfiguretorAsset.Count;
            for (int i = 0; i < numberOfLogType; i++)
            {

                if (_gameConfiguretorEnableStatus[i])
                    result += _listOfGameConfiguretorAsset[i].GetNumberOfLog(logType);
            }

            return result;
        }

        private string RemoveCoreDebugFromString(string context, GameConfiguratorAsset gameConfigAsset, LogType logType) {

            string result = "";

            if (context.Contains("color"))
            {
                Color color = Color.white;
                switch (logType) {

                    case LogType.Log:
                        color = gameConfigAsset.colorForLog;
                        break;

                    case LogType.Warning:
                        color = gameConfigAsset.colorForWarning;
                        break;

                    case LogType.Error:
                        color = gameConfigAsset.colorForLogError;
                        break;
                }
                string hexColor = StringOperation.GetHexColorFromRGBColor(color);

                context = context.Replace(string.Format("<color={0}>", hexColor), "");
                context = context.Replace("</color>", "");
            }

            string[] splitBy_ = context.Split('_');
            int numberOfSplit = splitBy_.Length;

            for (int i = 1; i < numberOfSplit; i++)
            {
                result += splitBy_[i];
                result += (i < (numberOfSplit - 1)) ? "_" : "";
            }

            return result;
        }

        private string GetButtonLabeledForGameConfiguretorSelection()
        {

            string result = "None";

            int numberOfSelectedAsset = 0;
            int numberOfGameConfiguretorAsset = _gameConfiguretorEnableStatus.Count;
            for (int i = 0; i < numberOfGameConfiguretorAsset; i++)
            {

                if (_gameConfiguretorEnableStatus[i])
                {

                    result = _gameConfiguretorOptionLabels[i];
                    numberOfSelectedAsset++;
                }
            }

            if (numberOfSelectedAsset > 1)
                result = "Mixed";

            return result;
        }

        private void ClearSelectedIndex() {

            _selectedLogIndex = -1;
        }

        private void ClearAllLog() {

            foreach (GameConfiguratorAsset gameConfiguratorAsset in _listOfGameConfiguretorAsset)
                gameConfiguratorAsset.ClearAllLog();
        }

        private void LogPlayModeState(PlayModeStateChange state)
        {

            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:

                    if (_isClearOnEnteringPlayMode)
                        ClearAllLog();

                    break;
            }
        }

        private void UpdateGameConfiguretorAsset() {

            _contentHeightForLogsInList = 30;

            _selectedLogColor = new Color(125 / 255.0f, 195 / 255.0f, 255 / 255.0f, 1f);
            defaultBackgroundColor = GUI.backgroundColor;
            defaultContentColor = GUI.contentColor;

            _GUIContentForClearDropdownButton.image = EditorGUIUtility.IconContent("Icon Dropdown").image;

            _GUIContentForTogglingInfoLog.image = EditorGUIUtility.IconContent("console.infoicon.sml").image;
            _GUIContentForTogglingWarningLog.image = EditorGUIUtility.IconContent("console.warnicon.sml").image;
            _GUIContentForTogglingErrorLog.image = EditorGUIUtility.IconContent("console.erroricon.sml").image;

            _GUIContentForInfoLog.image = EditorGUIUtility.IconContent("console.infoicon.sml@2x").image;
            _GUIContentForWarningLog.image = EditorGUIUtility.IconContent("console.warnicon.sml@2x").image;
            _GUIContentForErrorLog.image = EditorGUIUtility.IconContent("console.erroricon.sml@2x").image;


            _listOfGameConfiguretorAsset = CoreEditorModule.GetAsset<GameConfiguratorAsset>();

            int numberOfConfiguretorAsset = _listOfGameConfiguretorAsset.Count;

            _gameConfiguretorEnableStatus = new List<bool>();
            _gameConfiguretorOptionLabels = new List<string>();

            for (int i = 0; i < numberOfConfiguretorAsset; i++) {

                string prefix = _listOfGameConfiguretorAsset[i].prefix;
                if (string.IsNullOrEmpty(prefix) || string.IsNullOrWhiteSpace(prefix))
                {
                    prefix = _listOfGameConfiguretorAsset[i].name;
                    CoreDebugger.Debug.LogWarning("ScriptableObject name is assiged as prefix name as the 'prefix' field was empty : " + prefix);
                }

                _gameConfiguretorEnableStatus.Add(_listOfGameConfiguretorAsset[i].EditorAccessIfUsedByCentralGameConfiguretion);
                _gameConfiguretorOptionLabels.Add(prefix);

            }
        }

        private void CreateCoreConsole() {

            EditorApplication.playModeStateChanged += LogPlayModeState;


            UpdateGameConfiguretorAsset();

            string title = "Core Console";

            _editorWindowOfCoreConsole = GetWindow<CoreConsole>(title, typeof(CoreConsole));

            _editorWindowOfCoreConsole.titleContent.text = title;
            _editorWindowOfCoreConsole.minSize = new Vector2(480f, 240f);
            _editorWindowOfCoreConsole.Show();

        }

        #endregion

        #region CustomGUI

        private void HeaderGUI()
        {

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(50f)))
                {
                    ClearAllLog();
                }

                if (GUILayout.Button(_GUIContentForClearDropdownButton, EditorStyles.toolbarButton, GUILayout.Width(20))) {

                    GenericMenu genericMenuForClearMode = new GenericMenu();

                    int numberOfOption = _clearOptionLable.Length;
                    for (int i = 0; i < numberOfOption; i++) {

                        genericMenuForClearMode.AddItem(
                            new GUIContent(_clearOptionLable[i]),
                            _clearOptionStatus[i],
                            (index) => {
                                int selectedIndex = (int) index;   
                                _clearOptionStatus[selectedIndex] = !_clearOptionStatus[selectedIndex];
                            },
                            i);
                    }
                    genericMenuForClearMode.ShowAsContext();
                }


                Color defaultBackgroundColorOfGUI  = GUI.backgroundColor;
                Color dynamicColor          = defaultBackgroundColorOfGUI;

                dynamicColor.a              = _errorPause ? 1f : 0.5f;
                GUI.backgroundColor = dynamicColor;
                if (GUILayout.Button("Error Pause", GUILayout.Width(80))) {

                    _errorPause = !_errorPause;
                }
                GUI.backgroundColor = defaultBackgroundColorOfGUI;

                dynamicColor.a = _showTimeStamp ? 1f : 0.5f;
                GUI.backgroundColor = dynamicColor;
                if (GUILayout.Button("Time Stamp", GUILayout.Width(80)))
                {
                    _showTimeStamp = !_showTimeStamp;
                }
                GUI.backgroundColor = defaultBackgroundColorOfGUI;

                EditorGUILayout.LabelField("");

                //InfoLog
                DrawToggolingLogsGUI(LogType.Log);

                //WarningLog
                DrawToggolingLogsGUI(LogType.Warning);

                //ErrorLog
                DrawToggolingLogsGUI(LogType.Error);

                _GUIContentForSelectedConfigAsset.text = GetButtonLabeledForGameConfiguretorSelection();
                Vector2 sizeOfLabeledForSelectedConfigAsset = GUI.skin.label.CalcSize(_GUIContentForSelectedConfigAsset);
                EditorGUILayout.BeginHorizontal(GUILayout.Width(sizeOfLabeledForSelectedConfigAsset.x + 20));
                {
                    if (GUILayout.Button(_GUIContentForClearDropdownButton, EditorStyles.toolbarButton, GUILayout.Width(20)))
                    {

                        GenericMenu genericMenuForGameConfiguretorSelection = new GenericMenu();
                        int numberOfOption = _gameConfiguretorOptionLabels.Count;
                        for (int i = 0; i < numberOfOption; i++)
                        {

                            genericMenuForGameConfiguretorSelection.AddItem(
                                new GUIContent(_gameConfiguretorOptionLabels[i]),
                                _gameConfiguretorEnableStatus[i],
                                (index) => {
                                    int selectedIndex = (int)index;

                                    if (!_gameConfiguretorEnableStatus[selectedIndex] == true)
                                    {
                                        //if : Requested To Be True
                                        SerializedObject _soGameConfiguretorAsset = new SerializedObject(_listOfGameConfiguretorAsset[selectedIndex]);
                                        SerializedProperty _spEnableStackTrace = _soGameConfiguretorAsset.FindProperty("_enableStackTrace");

                                        if (!_spEnableStackTrace.boolValue)
                                        {
                                            // if : StackTrace is Disabled
                                            bool result = EditorUtility.DisplayDialog(
                                                "Enable StackTrace",
                                                "In order store and display the logs in 'CoreConsole', 'StackTrace' need to be enabled from 'GameConfiguretionAsset'",
                                                "Enable", "Cancel");

                                            _spEnableStackTrace.boolValue = result;
                                            _spEnableStackTrace.serializedObject.ApplyModifiedProperties();

                                            _gameConfiguretorEnableStatus[selectedIndex] = result;
                                        }
                                        else {

                                            _gameConfiguretorEnableStatus[selectedIndex] = !_gameConfiguretorEnableStatus[selectedIndex];
                                        }
                                    }
                                    else {

                                        _gameConfiguretorEnableStatus[selectedIndex] = !_gameConfiguretorEnableStatus[selectedIndex];
                                    }

                                    ClearSelectedIndex();
                                },
                                i);
                        }
                        genericMenuForGameConfiguretorSelection.ShowAsContext();
                    }

                    EditorGUILayout.LabelField(_GUIContentForSelectedConfigAsset, EditorStyles.toolbarButton, GUILayout.Width(sizeOfLabeledForSelectedConfigAsset.x + 10));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();

            CoreEditorModule.DrawHorizontalLine();
        }

        private void DrawToggolingLogsGUI(LogType logType)
        {
            Color defaultBackgroundColorOfGUI = GUI.backgroundColor;
            Color dynamicColor = defaultBackgroundColorOfGUI;
            float baseWidth = 15;
            switch (logType)
            {

                case LogType.Log:

                    _GUIContentForTogglingInfoLog.text = GetNumberOfLog(LogType.Log).ToString();
                    Vector2 sizeForInfoLogs = GUI.skin.label.CalcSize(_GUIContentForTogglingInfoLog);

                    dynamicColor.a = _enableInfoLog ? 1f : 0.5f;
                    GUI.backgroundColor = dynamicColor;
                    if (GUILayout.Button(_GUIContentForTogglingInfoLog, GUILayout.Width(baseWidth + sizeForInfoLogs.x)))
                    {
                        _enableInfoLog = !_enableInfoLog;
                        ClearSelectedIndex();
                    }
                    GUI.backgroundColor = defaultBackgroundColorOfGUI;

                    break;

                case LogType.Warning:

                    _GUIContentForTogglingWarningLog.text = GetNumberOfLog(LogType.Warning).ToString();
                    Vector2 sizeForWarningLog = GUI.skin.label.CalcSize(_GUIContentForTogglingWarningLog);

                    dynamicColor.a = _enableLogWarning ? 0.5f : 1f;
                    GUI.backgroundColor = dynamicColor;
                    if (GUILayout.Button(_GUIContentForTogglingWarningLog, GUILayout.Width(baseWidth + sizeForWarningLog.x)))
                    {
                        _enableLogWarning = !_enableLogWarning;
                        ClearSelectedIndex();
                    }
                    GUI.backgroundColor = defaultBackgroundColorOfGUI;

                    break;

                case LogType.Error:

                    _GUIContentForTogglingErrorLog.text = GetNumberOfLog(LogType.Error).ToString();
                    Vector2 sizeForErrorLog = GUI.skin.label.CalcSize(_GUIContentForTogglingErrorLog);

                    dynamicColor.a = _enableLogError ? 0.5f : 1f;
                    GUI.backgroundColor = dynamicColor;
                    if (GUILayout.Button(_GUIContentForTogglingErrorLog, GUILayout.Width(baseWidth + sizeForErrorLog.x)))
                    {
                        _enableLogError = !_enableLogError;
                        ClearSelectedIndex();
                    }
                    GUI.backgroundColor = defaultBackgroundColorOfGUI;

                    break;
            }
        }

        private void DrawLogListGUI() {

            EditorGUILayout.BeginVertical();
            {
                _scrollPositionForListOfLog = EditorGUILayout.BeginScrollView(_scrollPositionForListOfLog);
                {
                    GUIStyle GUIStyleForLogDetail = new GUIStyle(EditorStyles.toolbarButton);
                    GUIStyleForLogDetail.alignment = TextAnchor.MiddleLeft;
                    GUIStyleForLogDetail.fontSize = 12;
                    GUIStyleForLogDetail.fixedHeight = _contentHeightForLogsInList;

                    List<ConsoleDebugInfo> _listOfDebugInfo = new List<ConsoleDebugInfo>();

                    int numberOfGameConfiguretorAsset = _listOfGameConfiguretorAsset.Count;
                    for (int i = 0; i < numberOfGameConfiguretorAsset; i++) {

                        if (_gameConfiguretorEnableStatus[i]) {

                            foreach (CoreDebugger.Debug.DebugInfo debugInfo in _listOfGameConfiguretorAsset[i].EditorListOfLogInfo) {

                                _listOfDebugInfo.Add(new ConsoleDebugInfo() {
                                    gameConfig = _listOfGameConfiguretorAsset[i],
                                    debugInfo = debugInfo
                                });
                            }
                        }
                    }

                    //Sorting
                    int totalNumberOfLog = _listOfDebugInfo.Count;
                    for (int i = 0; i < totalNumberOfLog - 1; i++) {

                        System.DateTime _DataTimeWithWhomeToCompare = System.Convert.ToDateTime(_listOfDebugInfo[i].debugInfo.timeStamp);
                        for (int j = i + 1; j < totalNumberOfLog; j++) {

                            System.DateTime _DataTimeToCompare = System.Convert.ToDateTime(_listOfDebugInfo[j].debugInfo.timeStamp);

                            int compareValue = System.DateTime.Compare(_DataTimeWithWhomeToCompare, _DataTimeToCompare);
                            if (compareValue > 0) {

                                ConsoleDebugInfo tempValue = _listOfDebugInfo[i];
                                _listOfDebugInfo[i] = _listOfDebugInfo[j];
                                _listOfDebugInfo[j] = tempValue;
                            }
                        }
                    }

                    for (int i = 0; i < totalNumberOfLog; i++) {

                        DrawLog(_listOfDebugInfo[i].gameConfig, GUIStyleForLogDetail, _listOfDebugInfo[i].debugInfo, i);

                        EditorGUILayout.Space(5f);
                    }

                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawLog(
            GameConfiguratorAsset gameConfigAsset,
            GUIStyle GUIStyleForLog,
            CoreDebugger.Debug.DebugInfo debugInfo,
            int logIndex) {

            Color colorOfContent = defaultContentColor;
            GUIContent GUIContentForLabel = new GUIContent();
            switch (debugInfo.logType)
            {

                case LogType.Log:
                    colorOfContent = gameConfigAsset.colorForLog;
                    GUIContentForLabel = _GUIContentForInfoLog;
                    break;

                case LogType.Warning:
                    colorOfContent = gameConfigAsset.colorForWarning;
                    GUIContentForLabel = _GUIContentForWarningLog;
                    break;

                case LogType.Error:
                    colorOfContent = gameConfigAsset.colorForLogError;
                    GUIContentForLabel = _GUIContentForErrorLog;
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(GUIContentForLabel, GUILayout.Width(_contentHeightForLogsInList), GUILayout.Height(_contentHeightForLogsInList));

                string condition = RemoveCoreDebugFromString(
                    debugInfo.condition,
                    gameConfigAsset,
                    debugInfo.logType
                    );

                if (_showTimeStamp)
                    condition = string.Format("[{0}]_", debugInfo.timeStamp) + condition;

                GUI.backgroundColor = IsSelectedLog(logIndex) ? _selectedLogColor : defaultBackgroundColor;

                colorOfContent = colorOfContent == new Color() ? defaultContentColor : colorOfContent;
                GUI.contentColor = colorOfContent;
                if (GUILayout.Button(condition, GUIStyleForLog))
                {
                    _selectedLogIndex = logIndex;
                    _selectedLogCondition = debugInfo.condition;
                    _selectedLogStackTrace = debugInfo.stackTrace;
                }
                GUI.contentColor = defaultContentColor;
                GUI.backgroundColor = defaultBackgroundColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawLogMessageGUI()
        {
            
            EditorGUILayout.BeginVertical(GUILayout.Height(128));
            {
                _scrollPositionForLogMessage = EditorGUILayout.BeginScrollView(_scrollPositionForLogMessage);
                {
                    if (IsSelectedLog(_selectedLogIndex))
                    {
                        CoreEditorModule.DrawHorizontalLine();

                        _GUIContentForStackTrace.text = string.Format("{0}\n{1}", _selectedLogCondition, _selectedLogStackTrace);

                        Vector2 sizeOfLabel = GUI.skin.label.CalcSize(_GUIContentForStackTrace);
                        EditorGUILayout.LabelField(_GUIContentForStackTrace, GUILayout.Width(sizeOfLabel.x), GUILayout.Height(sizeOfLabel.y));
                    }
                    else {

                        CoreEditorModule.DrawHorizontalLine();

                        EditorGUILayout.HelpBox("No Valid Log Selected", MessageType.Warning);
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
            
        }

        #endregion
    }
}
