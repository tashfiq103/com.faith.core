namespace com.faith.core {
    using UnityEditor;
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;

    public class BaseEditorClass : Editor {

        protected static CoreEnums.CorePackageStatus _packageStatus = CoreEnums.CorePackageStatus.InDevelopment;

        #region OnEditor

        public virtual void OnEnable()
        {
            if(_packageStatus == CoreEnums.CorePackageStatus.InDevelopment)
                _packageStatus = AssetDatabase.FindAssets("com.faith.core", new string[] { "Packages" }).Length > 0 ? CoreEnums.CorePackageStatus.Production : CoreEnums.CorePackageStatus.InDevelopment;  
        }

        #endregion

        #region Editor Module   :   Asset


        protected List<T> GetAsset<T>(string nameFilter, bool returnIfGetAny = false, params string[] directoryFilters) {

            List<T> listOfAsset = new List<T>();
            string[] GUIDs;
            if (directoryFilters == null) GUIDs = AssetDatabase.FindAssets(nameFilter);
            else GUIDs = AssetDatabase.FindAssets(nameFilter, directoryFilters);

            foreach (string GUID in GUIDs) {

                string assetPath = AssetDatabase.GUIDToAssetPath(GUID);
                listOfAsset.Add((T) System.Convert.ChangeType(AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)), typeof(T)));
                if (returnIfGetAny)
                    break;
            }

            return listOfAsset;
        }

        #endregion

        #region Editor Module   :   GUI

        protected void DrawHorizontalLine () {
            EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
        }

        protected void DrawHorizontalLineOnGUI (Rect rect) {
            EditorGUI.LabelField (rect, "", GUI.skin.horizontalSlider);
        }

        protected void DrawSettingsEditor (Object settings, System.Action OnSettingsUpdated, ref bool foldout, ref Editor editor) {

            if (settings != null) {

                using (var check = new EditorGUI.ChangeCheckScope ()) {

                    foldout = EditorGUILayout.InspectorTitlebar (foldout, settings);

                    if (foldout) {

                        CreateCachedEditor (settings, null, ref editor);
                        editor.OnInspectorGUI ();

                        if (check.changed) {

                            if (OnSettingsUpdated != null) {

                                OnSettingsUpdated.Invoke ();
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