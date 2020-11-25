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

            CoreEditorModule.ShowScriptReference(serializedObject);

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

                sceneName.stringValue = CoreEditorModule.GetSceneNameFromPath(newPath);
                sceneName.serializedObject.ApplyModifiedProperties();

                if (CoreEditorModule.IsSceneAlreadyInBuild(newPath))
                {

                    isEnabled.boolValue = CoreEditorModule.IsSceneEnabled(newPath);
                }
            }


            if (newScene != null) {

                CoreEditorModule.DrawHorizontalLine();
                advanceOption.boolValue = EditorGUILayout.Foldout(
                    advanceOption.boolValue,
                    "Advance Option",
                    true
                );
                if (advanceOption.boolValue)
                {
                    EditorGUI.indentLevel += 1;

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(animationSpeedForLoadingBar);
                    if (EditorGUI.EndChangeCheck()) {

                        animationSpeedForLoadingBar.serializedObject.ApplyModifiedProperties();

                        bool usingConstant = animationSpeedForLoadingBar.FindPropertyRelative("UseConstant").boolValue;

                        if (usingConstant)
                        {
                            float clampedValue = animationSpeedForLoadingBar.FindPropertyRelative("ConstantValue").floatValue;

                            if (clampedValue < 0.1f || clampedValue > 1)
                            {
                                float willBeChangedValue = clampedValue;
                                clampedValue = Mathf.Clamp(willBeChangedValue, 0.1f, 1);
                                CoreDebugger.Debug.LogError(string.Format("animationValue need to be within the range of [0.1 , 1]. Changed '{0}' -> '{1}'", willBeChangedValue, clampedValue));

                            }

                            clampedValue = Mathf.Clamp(clampedValue, 0.1f, 1);

                            animationSpeedForLoadingBar.FindPropertyRelative("ConstantValue").floatValue = clampedValue;
                            animationSpeedForLoadingBar.FindPropertyRelative("ConstantValue").serializedObject.ApplyModifiedProperties();

                            animationSpeedForLoadingBar.serializedObject.ApplyModifiedProperties();
                        }
                        else {

                            if (animationSpeedForLoadingBar.FindPropertyRelative("Variable").objectReferenceValue != null)
                            {
                                SerializedObject floatVariable = new SerializedObject(animationSpeedForLoadingBar.FindPropertyRelative("Variable").objectReferenceValue);
                                
                                float clampedValue = floatVariable.FindProperty("Value").floatValue;

                                if (clampedValue < 0.1f || clampedValue > 1)
                                {
                                    float willBeChangedValue = clampedValue;
                                    clampedValue = Mathf.Clamp(willBeChangedValue, 0.1f, 1);
                                    CoreDebugger.Debug.LogError(string.Format("animationValue need to be within the range of [0.1 , 1]. Changed '{0}' -> '{1}'",willBeChangedValue, clampedValue));
                                    
                                }

                                floatVariable.FindProperty("DeveloperDescription").stringValue = "Value Should Be Within [0.1,1]";
                                floatVariable.FindProperty("DeveloperDescription").serializedObject.ApplyModifiedProperties();

                                floatVariable.FindProperty("Value").floatValue = clampedValue;
                                floatVariable.FindProperty("Value").serializedObject.ApplyModifiedProperties();

                                animationSpeedForLoadingBar.serializedObject.ApplyModifiedProperties();
                            }
                            else {

                                CoreDebugger.Debug.LogError("Please add 'SceneVariable' before modifying animationSpeed");
                            }
                        }
                    }

                    EditorGUILayout.PropertyField(loadSceneMode);
                    EditorGUI.indentLevel -= 1;
                }


                CoreEditorModule.DrawHorizontalLine();
                EditorGUILayout.BeginHorizontal();
                {
                    if (CoreEditorModule.IsSceneAlreadyInBuild(scenePath.stringValue))
                    {

                        EditorGUI.BeginChangeCheck();
                        isEnabled.boolValue = EditorGUILayout.Toggle(
                                "IsEnabled",
                                isEnabled.boolValue
                            );
                        if (EditorGUI.EndChangeCheck())
                        {

                            isEnabled.serializedObject.ApplyModifiedProperties();
                            CoreEditorModule.EnableAndDisableScene(scenePath.stringValue, isEnabled.boolValue);
                        }

                        if (GUILayout.Button("LoadScene")) {

                            _reference.LoadScene();
                        }
                        if (GUILayout.Button("Remove", GUILayout.Width(100)))
                        {
                            CoreEditorModule.RemoveSceneFromBuild(scenePath.stringValue);
                        }
                    }
                    else {

                        EditorGUILayout.HelpBox("Please add scene to the build settings", MessageType.Info);
                        if (GUILayout.Button("Add", GUILayout.Width(100))) {

                            CoreEditorModule.AddSceneToBuild(scenePath.stringValue, isEnabled.boolValue);
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


