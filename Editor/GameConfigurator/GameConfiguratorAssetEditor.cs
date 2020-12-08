namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(GameConfiguratorAsset))]
    public class GameConfiguratorAssetEditor : BaseEditorClass
    {

        #region Private Variables

        private GameConfiguratorAsset _reference;

        private SerializedProperty _sp_isUsedByCentralGameConfiguretion;
        private SerializedProperty _sp_linkWithCentralGameConfiguretion;

        private SerializedProperty _sp_enableStackTrace;
        private SerializedProperty _sp_numberOfLog;
        private SerializedProperty _sp_clearLogType;
        private SerializedProperty _sp_listOfLogInfo;

        private SerializedProperty _sp_gameMode;

        private SerializedProperty _sp_logType;
        private SerializedProperty _sp_prefix;
        private SerializedProperty _sp_colorForLog;
        private SerializedProperty _sp_colorForLogWarning;
        private SerializedProperty _sp_colorForLogError;

        private SerializedProperty _sp_dataSavingMode;

        private SerializedProperty _sp_dataSaveWhenSceneUnloaded;
        private SerializedProperty _sp_dataSaveWhenApplicationLoseFocus;
        private SerializedProperty _sp_dataSaveWhenApplicationQuit;
        private SerializedProperty _sp_snapshotFrequenceyInSec;

        #endregion


        public override void OnEnable()
        {
            base.OnEnable();

            if (target.GetType() != typeof(GameConfiguratorAsset))
                return;

            _reference = (GameConfiguratorAsset)target;

            _sp_isUsedByCentralGameConfiguretion = serializedObject.FindProperty("_isUsedByCentralGameConfiguretion");
            _sp_linkWithCentralGameConfiguretion = serializedObject.FindProperty("_linkWithCentralGameConfiguretion");

            _sp_enableStackTrace = serializedObject.FindProperty("_enableStackTrace");
            _sp_numberOfLog = serializedObject.FindProperty("_numberOfLog");
            _sp_clearLogType = serializedObject.FindProperty("_clearLogType");
            _sp_listOfLogInfo = serializedObject.FindProperty("_listOfLogInfo");

            _sp_gameMode = serializedObject.FindProperty("_gameMode");

            _sp_logType = serializedObject.FindProperty("_logType");
            _sp_prefix = serializedObject.FindProperty("prefix");
            _sp_colorForLog = serializedObject.FindProperty("colorForLog");
            _sp_colorForLogWarning = serializedObject.FindProperty("colorForWarning");
            _sp_colorForLogError = serializedObject.FindProperty("colorForLogError");

            _sp_dataSavingMode = serializedObject.FindProperty("_dataSavingMode");

            _sp_dataSaveWhenSceneUnloaded = serializedObject.FindProperty("dataSaveWhenSceneUnloaded");
            _sp_dataSaveWhenApplicationLoseFocus = serializedObject.FindProperty("dataSaveWhenApplicationLoseFocus");
            _sp_dataSaveWhenApplicationQuit = serializedObject.FindProperty("dataSaveWhenApplicationQuit");
            _sp_snapshotFrequenceyInSec = serializedObject.FindProperty("snapshotFrequenceyInSec");
        }

        public override void OnInspectorGUI()
        {
            CoreEditorModule.ShowScriptReference(serializedObject);

            serializedObject.Update();

            //Linking With Central Configuretor
            if (!_sp_isUsedByCentralGameConfiguretion.boolValue)
            {

                EditorGUILayout.PropertyField(_sp_linkWithCentralGameConfiguretion);
                CoreEditorModule.DrawHorizontalLine();
            }
            else {

                EditorGUILayout.HelpBox("The following configuretion asset is used in 'GameConfiguretionManager'.", MessageType.Info);
                CoreEditorModule.DrawHorizontalLine();
            }

            if (_sp_linkWithCentralGameConfiguretion.boolValue)
            {
                EditorGUILayout.HelpBox("The following configuretion is now synced with 'GameConfiguretionManager'. To make it standalone, unlink it", MessageType.Info);
            }
            else {

                EditorGUILayout.PropertyField(_sp_gameMode);
                CoreEditorModule.DrawHorizontalLine();

                EditorGUILayout.PropertyField(_sp_logType);
                EditorGUI.indentLevel += 1;
                switch (_sp_logType.enumValueIndex)
                {
                    case (int)CoreEnums.LogType.None:

                        break;
                    case (int)CoreEnums.LogType.Error:
                        EditorGUILayout.PropertyField(_sp_prefix);
                        EditorGUILayout.PropertyField(_sp_colorForLogError);
                        break;
                    case (int)CoreEnums.LogType.Info:
                        EditorGUILayout.PropertyField(_sp_prefix);
                        EditorGUILayout.PropertyField(_sp_colorForLog);
                        EditorGUILayout.PropertyField(_sp_colorForLogError);
                        break;
                    case (int)CoreEnums.LogType.Verbose:
                        EditorGUILayout.PropertyField(_sp_prefix);
                        EditorGUILayout.PropertyField(_sp_colorForLog);
                        EditorGUILayout.PropertyField(_sp_colorForLogWarning);
                        EditorGUILayout.PropertyField(_sp_colorForLogError);
                        break;
                }

                if (_sp_logType.enumValueIndex != 3) {

                    CoreEditorModule.DrawHorizontalLine();

                    EditorGUI.BeginChangeCheck();
                    _sp_enableStackTrace.boolValue = EditorGUILayout.Foldout(
                            _sp_enableStackTrace.boolValue,
                            "StackTrace",
                            true
                        );
                    if (EditorGUI.EndChangeCheck()) {

                        if (_sp_enableStackTrace.boolValue == false) {

                            _sp_listOfLogInfo.ClearArray();
                            _sp_listOfLogInfo.serializedObject.ApplyModifiedProperties();
                        }
                    }

                    if (_sp_enableStackTrace.boolValue) {

                        EditorGUI.indentLevel += 1;

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PropertyField(_sp_clearLogType);
                            if (GUILayout.Button("Clear", GUILayout.Width(75))){
                                _reference.ClearLog((LogType)_sp_clearLogType.enumValueIndex);
                            }

                            Color defaultContentColor = GUI.contentColor;
                            GUI.contentColor = Color.yellow;
                            EditorGUILayout.LabelField("|", EditorStyles.boldLabel, GUILayout.Width(5));
                            GUI.contentColor = defaultContentColor;

                            if (GUILayout.Button("ClearAll", GUILayout.Width(75)))
                            {
                                _reference.ClearAllLog();
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        if (!EditorApplication.isPlaying)
                            EditorGUILayout.PropertyField(_sp_numberOfLog);
                        else
                            EditorGUILayout.LabelField("MaxLogSize : " + _sp_numberOfLog.intValue, EditorStyles.boldLabel);

                        EditorGUI.BeginDisabledGroup(true);
                        {
                            EditorGUILayout.PropertyField(_sp_listOfLogInfo);
                        }
                        EditorGUI.EndDisabledGroup();

                        EditorGUI.indentLevel -= 1;
                    }
                }

                EditorGUI.indentLevel -= 1;
                CoreEditorModule.DrawHorizontalLine();

                if (_sp_isUsedByCentralGameConfiguretion.boolValue)
                {

                    if (_reference.dataSavingMode == CoreEnums.DataSavingMode.BinaryFormater)
                    {

                        CoreEditorModule.DrawHorizontalLine();
                        EditorGUILayout.HelpBox("Following data saving formate is still now in 'Preview', so things might get broken for different type of data saving", MessageType.Warning);

                        CoreEditorModule.DrawHorizontalLine();
                        EditorGUILayout.PropertyField(_sp_dataSavingMode);

                        EditorGUI.indentLevel += 1;

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(_sp_dataSaveWhenSceneUnloaded);
                        EditorGUILayout.PropertyField(_sp_dataSaveWhenApplicationLoseFocus);
                        EditorGUILayout.PropertyField(_sp_dataSaveWhenApplicationQuit);
                        EditorGUILayout.PropertyField(_sp_snapshotFrequenceyInSec);

                        EditorGUI.indentLevel -= 1;
                        CoreEditorModule.DrawHorizontalLine();
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(_sp_dataSavingMode);
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

