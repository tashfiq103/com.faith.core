namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(SceneVariable))]
    public class SceneVariableEditor : BaseEditorClass
    {
        #region Private Variables

        private SceneVariable _reference;

        private SerializedProperty advanceOption;
        private SerializedProperty isEnabled;
        private SerializedProperty scenePath;
        private SerializedProperty sceneName;
        private SerializedProperty animationSpeedForLoadingBar;
        private SerializedProperty loadSceneMode;

        #endregion

        #region Editor

        public override void OnEnable()
        {
            base.OnEnable();
            _reference = (SceneVariable)target;

            if (_reference == null)
                return;

            advanceOption = serializedObject.FindProperty("advanceOption");
            isEnabled = serializedObject.FindProperty("isEnabled");
            scenePath = serializedObject.FindProperty("scenePath");
            sceneName = serializedObject.FindProperty("sceneName");
            animationSpeedForLoadingBar = serializedObject.FindProperty("animationSpeedForLoadingBar");
            loadSceneMode = serializedObject.FindProperty("loadSceneMode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath.stringValue);
            EditorGUI.BeginChangeCheck();
            SceneAsset newScene = EditorGUILayout.ObjectField(
                    "Scene",
                    oldScene,
                    typeof(SceneAsset),
                    false
                ) as SceneAsset;
            if (EditorGUI.EndChangeCheck())
            {

                string newPath = AssetDatabase.GetAssetPath(newScene);

                scenePath.stringValue = newPath;
                scenePath.serializedObject.ApplyModifiedProperties();

                sceneName.stringValue = GetSceneNameFromPath(newPath);
                sceneName.serializedObject.ApplyModifiedProperties();

                if (IsSceneAlreadyInBuild(newPath))
                {

                    _reference.isEnabled = IsSceneEnabled(newPath);
                }
            }


            if (newScene != null) {

                DrawHorizontalLine();
                advanceOption.boolValue = EditorGUILayout.Foldout(
                    advanceOption.boolValue,
                    "Advance Option",
                    true
                );
                if (advanceOption.boolValue)
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUILayout.PropertyField(animationSpeedForLoadingBar);
                    EditorGUILayout.PropertyField(loadSceneMode);
                    EditorGUI.indentLevel -= 1;
                }


                DrawHorizontalLine();
                EditorGUILayout.BeginHorizontal();
                {
                    if (IsSceneAlreadyInBuild(_reference.scenePath))
                    {

                        EditorGUI.BeginChangeCheck();
                        isEnabled.boolValue = EditorGUILayout.Toggle(
                                "IsEnabled",
                                isEnabled.boolValue
                            );
                        if (EditorGUI.EndChangeCheck())
                        {

                            isEnabled.serializedObject.ApplyModifiedProperties();
                            EnableAndDisableScene(_reference.scenePath, isEnabled.boolValue);
                        }

                        if (GUILayout.Button("LoadScene")) {

                            _reference.LoadScene();
                        }
                        if (GUILayout.Button("Remove", GUILayout.Width(100)))
                        {
                            RemoveSceneFromBuild(_reference.scenePath);
                        }
                    }
                    else {

                        EditorGUILayout.HelpBox("Please add scene to the build settings", MessageType.Info);
                        if (GUILayout.Button("Add", GUILayout.Width(100))) {

                            AddSceneToBuild(_reference.scenePath, _reference.isEnabled);
                        }
                    }
                    
                }
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Configuretion

        

        #endregion
    }
}


