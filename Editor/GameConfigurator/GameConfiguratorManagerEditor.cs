
namespace com.faith.core
{
    using System.Collections.Generic;
    using UnityEngine;
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

        #region Configuretion

        private void ChangeConfiguretion() {

            List<GameConfiguratorAsset> listOfGameConfiguretorAsset = CoreEditorModule.GetAsset<GameConfiguratorAsset>();
            foreach (GameConfiguratorAsset asset in listOfGameConfiguretorAsset) {

                SerializedObject gameConfiguretionAsset = new SerializedObject(asset);

                if (_sp_gameConfiguratorAsset.objectReferenceValue == gameConfiguretionAsset.targetObject)
                {
                    gameConfiguretionAsset.FindProperty("_isUsedByCentralGameConfiguretion").boolValue = true;
                    gameConfiguretionAsset.FindProperty("_linkWithCentralGameConfiguretion").boolValue = false;
                    gameConfiguretionAsset.ApplyModifiedProperties();
                }
                else {

                    gameConfiguretionAsset.FindProperty("_isUsedByCentralGameConfiguretion").boolValue = false;
                    gameConfiguretionAsset.ApplyModifiedProperties();
                }

                //CoreDebugger.Debug.Log(gameConfiguretionAsset.targetObject.name + " : " + gameConfiguretionAsset.FindProperty("_isUsedByCentralGameConfiguretion").boolValue, Color.blue);
            }

        }

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
            CoreEditorModule.ShowScriptReference(serializedObject);

            serializedObject.Update();

            if (_packageStatus == CoreEnums.CorePackageStatus.InDevelopment) {

                EditorGUILayout.LabelField("PackageMode :   InDevelopment");
                CoreEditorModule.DrawHorizontalLine();
            }

            EditorGUILayout.PropertyField(_sp_instanceBehaviour);

            CoreEditorModule.DrawHorizontalLine();


            EditorGUI.BeginChangeCheck();
            _sp_gameConfiguratorAsset.objectReferenceValue = EditorGUILayout.ObjectField(
                    "Game Configuretor Asset",
                    _sp_gameConfiguratorAsset.objectReferenceValue,
                    typeof(GameConfiguratorAsset),
                    false
                ) as GameConfiguratorAsset;
            if (EditorGUI.EndChangeCheck()) {
                ChangeConfiguretion();
            }

            if (_reference.gameConfiguratorAsset != null)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.Space();
                CoreEditorModule.DrawSettingsEditor(_sp_gameConfiguratorAsset.objectReferenceValue, null, ref _reference.isGameConfiguratorAssetVisible, ref _gameConfiguratorAssetEditor);
                EditorGUI.indentLevel -= 1;
            }
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    }
}


