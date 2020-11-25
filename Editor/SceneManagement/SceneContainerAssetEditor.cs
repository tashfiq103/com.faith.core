namespace com.faith.core
{
    using UnityEditor;

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

            _reorderableListOfScene.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

    }
}

