namespace com.faith.core
{
    using UnityEngine;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(fileName = "SceneContainer", menuName = ScriptableObjectAssetMenu.MENU_SCENE_CONTAINER, order = ScriptableObjectAssetMenu.ORDER_SCENE_CONTAINER)]
    public class SceneContainerAsset : ScriptableObject
    {
        [SerializeField] private bool isActiveSceneListInBuildSettings = false;
        [SerializeField] private List<SceneReference> listOfScene;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SceneContainerAsset))]
    public class SceneContainerAssetEditor : BaseEditorClass
    {

        #region Private Variables
        private SceneContainerAsset _reference;

        private SerializedProperty _isActiveSceneListInBuildSettings;
        private SerializedProperty _listOfScene;

        private ReorderableList _reorderableListOfScene;
        #endregion




        #region Editor
        public override void OnEnable()
        {
            base.OnEnable();

            _reference = (SceneContainerAsset)target;

            if (_reference == null)
                return;

            _isActiveSceneListInBuildSettings = serializedObject.FindProperty("isActiveSceneListInBuildSettings");
            _listOfScene = serializedObject.FindProperty("listOfScene");

            _reorderableListOfScene = GetReorderableList(_listOfScene);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ShowScriptReference();

            _reorderableListOfScene.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Configuretion

        private void ChangeSceneStatus(bool addToBuild, bool removeUnlistedScene = false) {

            if (removeUnlistedScene) {


            }

            int numberOfScene = _listOfScene.arraySize;
            for (int i = 0; i < numberOfScene; i++) {

                SerializedObject element = _listOfScene.GetArrayElementAtIndex(i).serializedObject;

                string scenePath = element.FindProperty("scenePath").stringValue;
                
            }
        }

        #endregion

        #region CustomGUI

        private void DrawHeaderGUI() {

            if (_isActiveSceneListInBuildSettings.boolValue)
            {

                if (GUILayout.Button("Remove from Build"))
                {

                    _isActiveSceneListInBuildSettings.boolValue = false;
                }
            }
            else {

                if (GUILayout.Button("Add to Build"))
                {
                    
                    _isActiveSceneListInBuildSettings.boolValue = true;
                }
            }

            DrawHorizontalLine();
        }

        #endregion
    }

#endif
}

