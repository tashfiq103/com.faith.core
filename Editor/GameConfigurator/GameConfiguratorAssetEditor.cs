namespace com.faith.core
{
    using UnityEditor;

    [CustomEditor(typeof(GameConfiguratorAsset))]
    public class GameConfiguratorAssetEditor : BaseEditorClass
    {

        #region Private Variables

        private GameConfiguratorAsset _reference;

        private SerializedProperty _sp_isUsedByCentralGameConfiguretion;
        private SerializedProperty _sp_linkWithCentralGameConfiguretion;

        private SerializedProperty _sp_gameMode;

        private SerializedProperty _sp_logType;
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
            _reference = (GameConfiguratorAsset)target;

            if (_reference == null)
                return;

            _sp_isUsedByCentralGameConfiguretion = serializedObject.FindProperty("_isUsedByCentralGameConfiguretion");
            _sp_linkWithCentralGameConfiguretion = serializedObject.FindProperty("_linkWithCentralGameConfiguretion");

            _sp_gameMode = serializedObject.FindProperty("_gameMode");

            _sp_logType = serializedObject.FindProperty("_logType");
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
            ShowScriptReference();

            serializedObject.Update();

            //Linking With Central Configuretor
            if (!_sp_isUsedByCentralGameConfiguretion.boolValue)
            {

                EditorGUILayout.PropertyField(_sp_linkWithCentralGameConfiguretion);
                DrawHorizontalLine();
            }
            else {

                EditorGUILayout.HelpBox("The following configuretion asset is used in 'GameConfiguretionManager'.", MessageType.Info);
                DrawHorizontalLine();
            }

            if (_sp_linkWithCentralGameConfiguretion.boolValue)
            {
                EditorGUILayout.HelpBox("The following configuretion is now synced with 'GameConfiguretionManager'. To make it standalone, unlink it", MessageType.Info);
            }
            else {

                EditorGUILayout.PropertyField(_sp_gameMode);
                DrawHorizontalLine();

                EditorGUILayout.PropertyField(_sp_logType);
                EditorGUI.indentLevel += 1;
                switch (_sp_logType.enumValueIndex)
                {
                    case (int)CoreEnums.LogType.None:

                        break;
                    case (int)CoreEnums.LogType.Error:
                        EditorGUILayout.PropertyField(_sp_colorForLogError);
                        break;
                    case (int)CoreEnums.LogType.Info:
                        EditorGUILayout.PropertyField(_sp_colorForLog);
                        EditorGUILayout.PropertyField(_sp_colorForLogError);
                        break;
                    case (int)CoreEnums.LogType.Verbose:
                        EditorGUILayout.PropertyField(_sp_colorForLog);
                        EditorGUILayout.PropertyField(_sp_colorForLogWarning);
                        EditorGUILayout.PropertyField(_sp_colorForLogError);
                        break;
                }
                EditorGUI.indentLevel -= 1;
                DrawHorizontalLine();

                if (_sp_isUsedByCentralGameConfiguretion.boolValue)
                {

                    if (_reference.dataSavingMode == CoreEnums.DataSavingMode.BinaryFormater)
                    {

                        DrawHorizontalLine();
                        EditorGUILayout.HelpBox("Following data saving formate is still now in 'Preview', so things might get broken for different type of data saving", MessageType.Warning);

                        DrawHorizontalLine();
                        EditorGUILayout.PropertyField(_sp_dataSavingMode);

                        EditorGUI.indentLevel += 1;

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(_sp_dataSaveWhenSceneUnloaded);
                        EditorGUILayout.PropertyField(_sp_dataSaveWhenApplicationLoseFocus);
                        EditorGUILayout.PropertyField(_sp_dataSaveWhenApplicationQuit);
                        EditorGUILayout.PropertyField(_sp_snapshotFrequenceyInSec);

                        EditorGUI.indentLevel -= 1;
                        DrawHorizontalLine();
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

