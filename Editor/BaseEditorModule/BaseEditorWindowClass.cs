namespace com.faith.core
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    public class BaseEditorWindowClass : EditorWindow
    {

        protected static CoreEnums.CorePackageStatus _packageStatus = CoreEnums.CorePackageStatus.InDevelopment;

        #region OnEditor

        public virtual void OnEnable()
        {
            if (_packageStatus == CoreEnums.CorePackageStatus.InDevelopment)
                _packageStatus = AssetDatabase.FindAssets("com.faith.core", new string[] { "Packages" }).Length > 0 ? CoreEnums.CorePackageStatus.Production : CoreEnums.CorePackageStatus.InDevelopment;
        }

        #endregion

        #region Editor Module   :   Asset

        protected static List<T> GetAsset<T>(bool returnIfGetAny = false, params string[] directoryFilters)
        {

            return GetAsset<T>("t:" + typeof(T).ToString().Replace("UnityEngine.", ""), returnIfGetAny, directoryFilters);
        }

        public static List<T> GetAsset<T>(string nameFilter, bool returnIfGetAny = false, params string[] directoryFilters)
        {

            List<T> listOfAsset = new List<T>();
            string[] GUIDs;
            if (directoryFilters == null) GUIDs = AssetDatabase.FindAssets(nameFilter);
            else GUIDs = AssetDatabase.FindAssets(nameFilter, directoryFilters);

            foreach (string GUID in GUIDs)
            {

                string assetPath = AssetDatabase.GUIDToAssetPath(GUID);
                listOfAsset.Add((T)System.Convert.ChangeType(AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)), typeof(T)));
                if (returnIfGetAny)
                    break;
            }

            return listOfAsset;
        }

        #endregion


        #region Editor Module

        public void DrawHorizontalLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public void DrawHorizontalLineOnGUI(Rect rect)
        {
            EditorGUI.LabelField(rect, "", GUI.skin.horizontalSlider);
        }

        public void DrawSettingsEditor(Object settings, System.Action OnSettingsUpdated, ref bool foldout, ref Editor editor, bool drawInspector = true)
        {

            if (settings != null)
            {

                using (var check = new EditorGUI.ChangeCheckScope())
                {

                    foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

                    if (foldout)
                    {

                        UnityEditor.Editor.CreateCachedEditor(settings, null, ref editor);
                        if(drawInspector)
                            editor.OnInspectorGUI();

                        if (check.changed)
                        {

                            if (OnSettingsUpdated != null)
                            {

                                OnSettingsUpdated.Invoke();
                            }
                        }
                    }
                }
            }

        }

        #endregion
    }
}