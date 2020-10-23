namespace com.faith.core
{
    using UnityEditor;

    [CustomEditor(typeof(GameConfiguratorAsset))]
    public class GameConfiguratorAssetEditor : BaseEditorClass
    {

        #region Private Variables

        private GameConfiguratorAsset _reference;

        private SerializedProperty _sp_gameMode;
        private SerializedProperty _sp_logType;
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

            _sp_gameMode = serializedObject.FindProperty("gameMode");
            _sp_logType = serializedObject.FindProperty("logType");
            _sp_dataSavingMode = serializedObject.FindProperty("dataSavingMode");

            _sp_dataSaveWhenSceneUnloaded = serializedObject.FindProperty("dataSaveWhenSceneUnloaded");
            _sp_dataSaveWhenApplicationLoseFocus = serializedObject.FindProperty("dataSaveWhenApplicationLoseFocus");
            _sp_dataSaveWhenApplicationQuit = serializedObject.FindProperty("dataSaveWhenApplicationQuit");
            _sp_snapshotFrequenceyInSec = serializedObject.FindProperty("snapshotFrequenceyInSec");
        }

        public override void OnInspectorGUI()
        {
            ShowScriptReference();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_sp_gameMode);
            EditorGUILayout.PropertyField(_sp_logType);
            

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
            else {
                EditorGUILayout.PropertyField(_sp_dataSavingMode);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

