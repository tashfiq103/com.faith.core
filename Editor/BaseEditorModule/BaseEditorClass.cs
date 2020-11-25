namespace com.faith.core {
    using UnityEditor;
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;

    public class BaseEditorClass : Editor {

        protected static CoreEnums.CorePackageStatus _packageStatus = CoreEnums.CorePackageStatus.InDevelopment;
        protected float singleLineHeight;

        #region CustomEditorClass

        protected class ReorderableList
        {

            #region Private Variables

            private UnityEditorInternal.ReorderableList _reorderableList;
            private SerializedObject _serializedObject;
            private SerializedProperty _sourceProperty;
            private bool _isFoldout = false;
            private int _popUpValue = 0;
            private string[] _popupOptions = new string[] { "Generic", "Reorderable" };
            #endregion

            #region Configuretion

            private void SaveModifiedProperties()
            {

                _serializedObject.ApplyModifiedProperties();
                _sourceProperty.serializedObject.ApplyModifiedProperties();
            }

            private void DrawGenericList()
            {

                _isFoldout = EditorGUILayout.Foldout(
                        _isFoldout,
                        _sourceProperty.displayName,
                        true
                    );
                if (_isFoldout)
                {

                    EditorGUI.indentLevel += 1;
                    EditorGUI.BeginChangeCheck();
                    _sourceProperty.arraySize = EditorGUILayout.IntField("Size", _sourceProperty.arraySize);
                    if (EditorGUI.EndChangeCheck())
                    {
                        SaveModifiedProperties();
                    }
                    EditorGUI.indentLevel -= 1;

                    for (int i = 0; i < _sourceProperty.arraySize; i++)
                    {

                        EditorGUI.indentLevel += 1;
                        EditorGUILayout.PropertyField(_sourceProperty.GetArrayElementAtIndex(i), true);
                        EditorGUI.indentLevel -= 1;
                    }
                }
            }

            #endregion

            #region Public Callback

            public ReorderableList(SerializedObject serializedObject, SerializedProperty sourceProperty, bool drawLineSeperator = false)
            {
                _serializedObject = serializedObject;
                _sourceProperty = sourceProperty;
                float singleLineHeight = EditorGUIUtility.singleLineHeight;

                _reorderableList = new UnityEditorInternal.ReorderableList(_serializedObject, _sourceProperty)
                {

                    displayAdd = true,
                    displayRemove = true,
                    draggable = true,
                    drawHeaderCallback = rect =>
                    {
                        _isFoldout = EditorGUI.Foldout(
                                new Rect(rect.x + 12, rect.y, rect.width, singleLineHeight),
                                _isFoldout,
                                _sourceProperty.displayName,
                                true
                            );
                    },
                    drawElementCallback = (rect, index, isActive, isFocused) => {

                        SerializedProperty element = _sourceProperty.GetArrayElementAtIndex(index);
                        float heightOfElement = EditorGUI.GetPropertyHeight(element);

                        if (_isFoldout)
                        {

                            EditorGUI.PropertyField(
                                new Rect(rect.x + 12, rect.y, rect.width, heightOfElement),
                                element,
                                true
                            );

                            if (drawLineSeperator)
                                EditorGUI.LabelField(new Rect(rect.x, rect.y + heightOfElement, rect.width, singleLineHeight), "", GUI.skin.horizontalSlider);
                        }



                    },
                    elementHeightCallback = index => {

                        return _isFoldout ? EditorGUI.GetPropertyHeight(_sourceProperty.GetArrayElementAtIndex(index)) + (drawLineSeperator ? singleLineHeight : 0) : 0;
                    }
                };
            }

            public void DoLayoutList()
            {

                if (!_isFoldout)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        _isFoldout = EditorGUILayout.Foldout(
                                _isFoldout,
                                _sourceProperty.displayName,
                                true
                            );

                        EditorGUI.BeginChangeCheck();
                        _popUpValue = EditorGUILayout.Popup(
                                _popUpValue,
                                _popupOptions,
                                GUILayout.Width(100)
                            );
                    }
                    EditorGUILayout.EndHorizontal();

                }
                else
                {
                    if (_popUpValue == 0) DrawGenericList();
                    else _reorderableList.DoLayoutList();
                }
            }

            #endregion

        }

        #endregion

        #region OnEditor

        public virtual void OnEnable()
        {
            if (_packageStatus == CoreEnums.CorePackageStatus.InDevelopment)
                _packageStatus = AssetDatabase.FindAssets("com.faith.core", new string[] { "Packages" }).Length > 0 ? CoreEnums.CorePackageStatus.Production : CoreEnums.CorePackageStatus.InDevelopment;

            singleLineHeight = EditorGUIUtility.singleLineHeight;

        }

        #endregion

        #region Editor Module   :   Asset

        protected List<T> GetAsset<T>(bool returnIfGetAny = false, params string[] directoryFilters) {

            return GetAsset<T>("t:" + typeof(T).ToString().Replace("UnityEngine.", ""), returnIfGetAny, directoryFilters);
        }

        protected List<T> GetAsset<T>(string nameFilter, bool returnIfGetAny = false, params string[] directoryFilters) {

            List<T> listOfAsset = new List<T>();
            string[] GUIDs;
            if (directoryFilters == null) GUIDs = AssetDatabase.FindAssets(nameFilter);
            else GUIDs = AssetDatabase.FindAssets(nameFilter, directoryFilters);
            
            foreach (string GUID in GUIDs) {

                string assetPath = AssetDatabase.GUIDToAssetPath(GUID);
                listOfAsset.Add((T)System.Convert.ChangeType(AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)), typeof(T)));
                if (returnIfGetAny)
                    break;
            }

            return listOfAsset;
        }

        #endregion

        #region Editor Module   :   GUI

        protected void ShowScriptReference() {

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
        }

        protected void DrawHorizontalLine() {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        protected void DrawHorizontalLineOnGUI(Rect rect) {
            EditorGUI.LabelField(rect, "", GUI.skin.horizontalSlider);
        }

        protected void DrawSettingsEditor(Object settings, System.Action OnSettingsUpdated, ref bool foldout, ref Editor editor) {

            if (settings != null) {

                using (var check = new EditorGUI.ChangeCheckScope()) {

                    foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

                    if (foldout) {

                        CreateCachedEditor(settings, null, ref editor);
                        editor.OnInspectorGUI();

                        if (check.changed) {

                            if (OnSettingsUpdated != null) {

                                OnSettingsUpdated.Invoke();
                            }
                        }
                    }
                }
            }

        }


        #endregion

        #region Editor Module   :   Scene

        protected string GetSceneNameFromPath(string scenePath) {

            string[] splitedByDash = scenePath.Split('/');
            string[] splitedByDot = splitedByDash[splitedByDash.Length - 1].Split('.');
            return splitedByDot[0];
        }

        protected bool IsSceneAlreadyInBuild(string scenePath)
        {
            int numberOfSceneInBuild = EditorBuildSettings.scenes.Length;
            for (int i = 0; i < numberOfSceneInBuild; i++)
            {
                if (EditorBuildSettings.scenes[i].path == scenePath)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool IsSceneEnabled(string scenePath) {


            if (IsSceneAlreadyInBuild(scenePath)) {

                EditorBuildSettingsScene[] edtiorBuildSettingsScene = EditorBuildSettings.scenes;
                foreach (EditorBuildSettingsScene buildScene in edtiorBuildSettingsScene)
                {
                    if (buildScene.path == scenePath && buildScene.enabled)
                        return true;
                }
            }

            return false;
        }

        protected void EnableAndDisableScene(string scenePath, bool value) {

            if (IsSceneAlreadyInBuild(scenePath)) {

                EditorBuildSettingsScene[] editorBuildSettingsScene = EditorBuildSettings.scenes;
                foreach (EditorBuildSettingsScene buildScene in editorBuildSettingsScene)
                {
                    if (buildScene.path == scenePath) {

                        buildScene.enabled = value;
                        break;
                    }
                }
                EditorBuildSettings.scenes = editorBuildSettingsScene;

            }
        }

        protected void AddSceneToBuild(string scenePath, bool isEnabled = true) {

            EditorBuildSettingsScene newBuildScene = new EditorBuildSettingsScene(scenePath, isEnabled);
            List<EditorBuildSettingsScene> tempBuildSettingsScene = EditorBuildSettings.scenes.ToList();
            tempBuildSettingsScene.Add(newBuildScene);
            EditorBuildSettings.scenes = tempBuildSettingsScene.ToArray();
        }

        protected void RemoveSceneFromBuild(string t_ScenePath)
        {
            List<EditorBuildSettingsScene> tempBuildSettingsScene = EditorBuildSettings.scenes.ToList();
            int numberOfCurrentSceneInTheBuild = tempBuildSettingsScene.Count;
            for (int i = 0; i < numberOfCurrentSceneInTheBuild; i++)
            {
                if (tempBuildSettingsScene[i].path == t_ScenePath)
                {
                    tempBuildSettingsScene.RemoveAt(i);
                    tempBuildSettingsScene.TrimExcess();
                    break;
                }
            }
            EditorBuildSettings.scenes = tempBuildSettingsScene.ToArray();
        }

        #endregion

    }
}