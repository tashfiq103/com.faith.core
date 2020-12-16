namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using System.Collections.Generic;
    

    public class CoreConsole : BaseEditorWindowClass, IPreprocessBuildWithReport
    {

        #region Public Variables

        public int callbackOrder => throw new System.NotImplementedException();

        #endregion

        #region Private Variables

        private static List<CoreConsole> _listOfCoreConsole = new List<CoreConsole>();
        private static List<GameConfiguratorAsset> _listOfGameConfiguretorAsset;

        private CoreConsole _editorWindowOfCoreConsole;

        private GUIContent _GUIContentForClearDropdownButton= new GUIContent();

        private GUIContent _GUIContentForTogglingInfoLog = new GUIContent();
        private GUIContent _GUIContentForTogglingWarningLog = new GUIContent();
        private GUIContent _GUIContentForTogglingErrorLog = new GUIContent();

        private GUIContent _GUIContentForInfoLog = new GUIContent();
        private GUIContent _GUIContentForWarningLog = new GUIContent();
        private GUIContent _GUIContentForErrorLog = new GUIContent();

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

        private Vector2 _selectedIndex;
        private Vector2 _scrollPositionForListOfLog;
        private Vector2 _scrollPositionForStackTraceInfo;

        private Color _selectedLogColor;

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

        private bool IsValidGameConfigAssetIndex(int gameConfigIndex) {

            if (gameConfigIndex >= 0 && gameConfigIndex < _listOfGameConfiguretorAsset.Count)
                return true;

            return false;
        }

        private bool IsValidLogIndex(int gameConfigIndex, int logIndex) {

            if (IsValidGameConfigAssetIndex(gameConfigIndex) && (logIndex >= 0 && logIndex < _listOfGameConfiguretorAsset[gameConfigIndex].NumberOfLog))
                return true;

            return false;
        }

        private bool IsSelectedLog(int gameConfigIndex, int logIndex)
        {
            if (IsValidLogIndex(gameConfigIndex, logIndex) && gameConfigIndex == _selectedIndex.x && logIndex == _selectedIndex.y)
                return true;

            return false;
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

        private int GetNumberOfLog(LogType logType) {

            int result = 0;
            int numberOfLogType = _listOfGameConfiguretorAsset.Count;
            for (int i = 0; i < numberOfLogType; i++) {

                if(_gameConfiguretorEnableStatus[i])
                    result += _listOfGameConfiguretorAsset[i].GetNumberOfLog(logType);
            }

            return result;
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
            _editorWindowOfCoreConsole.minSize = new Vector2(360f, 240f);
            _editorWindowOfCoreConsole.Show();

        }

        #endregion

        #region CustomGUI

        private void DrawLog(CoreDebugger.Debug.DebugInfo debugInfo, GameConfiguratorAsset gameConfiguretorAsset) {

        }

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

                dynamicColor.a              = _errorPause ? 0.5f : 1f;
                GUI.backgroundColor = dynamicColor;
                if (GUILayout.Button("Error Pause", GUILayout.Width(80))) {

                    _errorPause = !_errorPause;
                }
                GUI.backgroundColor = defaultBackgroundColorOfGUI;

                dynamicColor.a = _showTimeStamp ? 0.5f : 1f;
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

                string selectedTitle    = GetButtonLabeledForGameConfiguretorSelection();
                float contentWidth      = selectedTitle.Length * 8f;
                EditorGUILayout.BeginHorizontal(GUILayout.Width(contentWidth + 20));
                {
                    EditorGUILayout.LabelField(selectedTitle, EditorStyles.toolbarButton, GUILayout.Width(contentWidth));

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
                                    _gameConfiguretorEnableStatus[selectedIndex] = !_gameConfiguretorEnableStatus[selectedIndex];
                                },
                                i);
                        }
                        genericMenuForGameConfiguretorSelection.ShowAsContext();
                    }
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

                    dynamicColor.a = _enableInfoLog ? 0.5f : 1f;
                    GUI.backgroundColor = dynamicColor;
                    if (GUILayout.Button(_GUIContentForTogglingInfoLog, GUILayout.Width(baseWidth + sizeForInfoLogs.x)))
                    {
                        _enableInfoLog = !_enableInfoLog;
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
                    GUIStyle logGUIStyle = new GUIStyle(EditorStyles.toolbarButton);
                    logGUIStyle.alignment = TextAnchor.MiddleLeft;
                    logGUIStyle.fontSize = 12;
                    logGUIStyle.fixedHeight = _contentHeightForLogsInList;

                    Color defaultBackgroundColor = GUI.backgroundColor;
                    Color defaultContentColor = GUI.contentColor;

                    int numberOfGameConfiguretorAsset = _listOfGameConfiguretorAsset.Count;
                    for (int i = 0; i < numberOfGameConfiguretorAsset; i++)
                    {
                        if (_gameConfiguretorEnableStatus[i])
                        {

                            SerializedObject gameConfiguretorAsset  = new SerializedObject(_listOfGameConfiguretorAsset[i]);
                            SerializedProperty _listOfLogInfo       = gameConfiguretorAsset.FindProperty("_listOfLogInfo");

                            int numberOfLog = _listOfLogInfo.arraySize;
                            for (int j = 0; j < numberOfLog; j++)
                            {

                                LogType logType = (LogType)_listOfLogInfo.GetArrayElementAtIndex(j).FindPropertyRelative("logType").enumValueIndex;
                                switch (logType) {

                                    case LogType.Log:

                                        if (_enableInfoLog)
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField(_GUIContentForInfoLog, GUILayout.Width(_contentHeightForLogsInList), GUILayout.Height(_contentHeightForLogsInList));

                                                string condition = RemoveCoreDebugFromString(
                                                    _listOfLogInfo.GetArrayElementAtIndex(j).FindPropertyRelative("condition").stringValue,
                                                    _listOfGameConfiguretorAsset[i],
                                                    (LogType) _listOfLogInfo.GetArrayElementAtIndex(j).FindPropertyRelative("logType").enumValueIndex
                                                    );

                                                GUI.backgroundColor = IsSelectedLog(i, j) ? _selectedLogColor : defaultBackgroundColor;

                                                Color colorOfContent = _listOfGameConfiguretorAsset[i].colorForLog;
                                                colorOfContent = colorOfContent == new Color() ? defaultContentColor : _listOfGameConfiguretorAsset[i].colorForLog;
                                                GUI.contentColor = colorOfContent;
                                                if (GUILayout.Button(condition, logGUIStyle)) {

                                                    _selectedIndex = new Vector2(i, j);
                                                }
                                                GUI.contentColor = defaultContentColor;
                                                GUI.backgroundColor = defaultBackgroundColor;
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }

                                        break;

                                    case LogType.Warning:

                                        if (_enableLogWarning)
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField(_GUIContentForWarningLog, GUILayout.Width(_contentHeightForLogsInList), GUILayout.Height(_contentHeightForLogsInList));

                                                string condition = RemoveCoreDebugFromString(
                                                    _listOfLogInfo.GetArrayElementAtIndex(j).FindPropertyRelative("condition").stringValue,
                                                    _listOfGameConfiguretorAsset[i],
                                                    (LogType)_listOfLogInfo.GetArrayElementAtIndex(j).FindPropertyRelative("logType").enumValueIndex
                                                );

                                                GUI.backgroundColor = IsSelectedLog(i, j) ? _selectedLogColor : defaultBackgroundColor;
                                                GUI.contentColor = _listOfGameConfiguretorAsset[i].colorForWarning;
                                                if (GUILayout.Button(condition, logGUIStyle))
                                                {
                                                    _selectedIndex = new Vector2(i, j);
                                                }
                                                GUI.contentColor = defaultContentColor;
                                                GUI.backgroundColor = defaultBackgroundColor;
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }

                                        break;

                                    case LogType.Error:

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField(_GUIContentForErrorLog, GUILayout.Width(_contentHeightForLogsInList), GUILayout.Height(_contentHeightForLogsInList));

                                            string condition = RemoveCoreDebugFromString(
                                                _listOfLogInfo.GetArrayElementAtIndex(j).FindPropertyRelative("condition").stringValue,
                                                _listOfGameConfiguretorAsset[i],
                                                (LogType)_listOfLogInfo.GetArrayElementAtIndex(j).FindPropertyRelative("logType").enumValueIndex
                                            );

                                            GUI.backgroundColor = IsSelectedLog(i, j) ? _selectedLogColor : defaultBackgroundColor;
                                            GUI.contentColor = _listOfGameConfiguretorAsset[i].colorForLogError;
                                            if (GUILayout.Button(condition, logGUIStyle))
                                            {
                                                _selectedIndex = new Vector2(i, j);
                                            }
                                            GUI.contentColor = defaultContentColor;
                                            GUI.backgroundColor = defaultBackgroundColor;
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        break;
                                }
                                EditorGUILayout.Space(5f);
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}
