
namespace com.faith.core
{
    using UnityEditor;

    [CustomEditor(typeof(GameConfiguratorManager))]
    public class GameConfiguratorManagerEditor : BaseEditorClass
    {
        #region Private Variables

        private GameConfiguratorManager _reference;

        private Editor  _gameConfiguratorAssetEditor;

        private SerializedProperty _sp_instanceBehaviour;
        private SerializedProperty _sp_gameConfiguratorAsset;

        #endregion


        #region OnEditor

        public override void OnEnable()
        {

            base.OnEnable();

            _reference = (GameConfiguratorManager)target;

            if (_reference == null)
                return;

            _sp_instanceBehaviour = serializedObject.FindProperty("instanceBehaviour");
            _sp_gameConfiguratorAsset = serializedObject.FindProperty("gameConfiguratorAsset");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_packageStatus == CoreEnums.CorePackageStatus.InDevelopment) {

                EditorGUILayout.LabelField("PackageMode :   InDevelopment");
                DrawHorizontalLine();
            }

            EditorGUILayout.PropertyField(_sp_instanceBehaviour);

            DrawHorizontalLine();
            EditorGUILayout.PropertyField(_sp_gameConfiguratorAsset);

            if (_reference.gameConfiguratorAsset != null)
            {
                EditorGUI.indentLevel += 1;
                DrawSettingsEditor(_sp_gameConfiguratorAsset.objectReferenceValue, null, ref _reference.isGameConfiguratorAssetVisible, ref _gameConfiguratorAssetEditor);
                EditorGUI.indentLevel -= 1;
            }
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    }
}


