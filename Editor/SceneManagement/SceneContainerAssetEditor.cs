namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    [CustomEditor(typeof(SceneContainerAsset))]
    public class SceneContainerAssetEditor : BaseEditorClass
    {

        #region Private Variables
        private SceneContainerAsset _reference;

        private SerializedProperty _listOfScene;

        private CoreEditorModule.ReorderableList _reorderableListOfScene;
        #endregion


        #region Editor
        public override void OnEnable()
        {
            base.OnEnable();

            _reference = (SceneContainerAsset)target;

            if (_reference == null)
                return;

            _listOfScene = serializedObject.FindProperty("listOfScene");

            _reorderableListOfScene = new CoreEditorModule.ReorderableList(serializedObject,  _listOfScene);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CoreEditorModule.ShowScriptReference(serializedObject);

            if (GUILayout.Button("PushToBuild")) {

                PushToBuild();
            }

            _reorderableListOfScene.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Configuretion

        private void PushToBuild() {

            SceneManagementEditorWindow.productionSceneContainer = _reference;

            List<EditorBuildSettingsScene> editorBuildSettingsScene = new List<EditorBuildSettingsScene>();
            int arraySize = _listOfScene.arraySize;
            for (int i = 0; i < arraySize; i++) {

                string scenePath = _listOfScene.GetArrayElementAtIndex(i).FindPropertyRelative("scenePath").stringValue;
                editorBuildSettingsScene.Add(new EditorBuildSettingsScene(scenePath, true));
            }
            EditorBuildSettings.scenes = editorBuildSettingsScene.ToArray();
        }

        #endregion

    }
}

