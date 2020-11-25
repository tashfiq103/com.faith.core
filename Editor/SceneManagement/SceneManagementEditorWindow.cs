namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Collections.Generic;

    public class SceneManagementEditorWindow    :   BaseEditorWindowClass
    {
        #region Private Variables

        private static SceneManagementEditorWindow EditorWindow;

        private static SceneContainerAsset          _productionSceneContainer;
        private static List<SceneContainerAsset>    _listOfSceneContainerAsset;

        private static SerializedObject[]           _serializedObjectOfSceneContainerAsset;
        private static SerializedProperty[]         _serializedPropertyOfSceneList;
        private static CoreEditorModule.ReorderableList[]            _reorderableListOfSceneContainerAsset;

        private static Editor[]                     _editorForSceneContainerAsset;

        private static bool[] _isFoldOut;
        private static bool _isFoldOutOtherSceneContainerAsset;

        private static int                          _productionSceneIndex;
        private static int                          _numberOfSceneContainerAsset;
        private static int[]                        _popUpOption;

        private const string _defaultName = "NewSceneContainer";
        private static string _nameOfSceneContainer = _defaultName;
        private static string[] _viewMode = new string[] { "BrowsingView", "ConfigView" };

        private static GUIStyle WhiteBackgroundGUIStyle = new GUIStyle();

        private Color colorForHeader = new Color(0, 1, 1, 0.1f);

        #endregion

        #region Configuretion

        private void AddSceneContainerAsset() {

            if (!Directory.Exists(CoreConstant.DirectoryForSceneContainerAsset))
                Directory.CreateDirectory(CoreConstant.DirectoryForSceneContainerAsset);

            _nameOfSceneContainer       = _nameOfSceneContainer.Length == 0 ? _defaultName : _nameOfSceneContainer;
            int numberOfDuplicateName   = IsThereAnySceneContainerWithTheGivenName(_nameOfSceneContainer);
            string absoluteName         = _nameOfSceneContainer + (numberOfDuplicateName == 0 ? "" : (" " + numberOfDuplicateName));

            SceneContainerAsset newSceneContainerAsset = ScriptableObject.CreateInstance<SceneContainerAsset>();

            AssetDatabase.CreateAsset(newSceneContainerAsset, CoreConstant.DirectoryForSceneContainerAsset + "/" + absoluteName + ".asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = newSceneContainerAsset;

            UpdateListOfSceneContainerAsset();
        }

        private static int IsThereAnySceneContainerWithTheGivenName(string name)
        {
            int _numberOfDuplicateName = 0;
            List<SceneContainerAsset> sceneContainerAssets = CoreEditorModule.GetAsset<SceneContainerAsset>();
            foreach (SceneContainerAsset sceneContainerAsset in sceneContainerAssets)
            {
                if (sceneContainerAsset.name.Contains(name))
                    _numberOfDuplicateName++;
            }

            return _numberOfDuplicateName;
        }

        private static void UpdateListOfSceneContainerAsset()
        {
            _listOfSceneContainerAsset      = CoreEditorModule.GetAsset<SceneContainerAsset>();
            _numberOfSceneContainerAsset    = _listOfSceneContainerAsset.Count;

            _popUpOption                    = new int[_numberOfSceneContainerAsset];
            _isFoldOut                      = new bool[_numberOfSceneContainerAsset];
            _editorForSceneContainerAsset   = new Editor[_numberOfSceneContainerAsset];

            _serializedObjectOfSceneContainerAsset = new SerializedObject[_numberOfSceneContainerAsset];
            _serializedPropertyOfSceneList = new SerializedProperty[_numberOfSceneContainerAsset];
            _reorderableListOfSceneContainerAsset = new CoreEditorModule.ReorderableList[_numberOfSceneContainerAsset];

            for (int i = 0; i < _numberOfSceneContainerAsset; i++) {

                _serializedObjectOfSceneContainerAsset[i] = new SerializedObject(_listOfSceneContainerAsset[i]);
                _serializedPropertyOfSceneList[i] = _serializedObjectOfSceneContainerAsset[i].FindProperty("listOfScene");

                _reorderableListOfSceneContainerAsset[i] = new CoreEditorModule.ReorderableList(_serializedObjectOfSceneContainerAsset[i], _serializedPropertyOfSceneList[i], true);
            }
        }

        private void DrawSceneContainer(int index, ref SceneContainerAsset sceneContainer, Color color = new Color()) {

            SerializedObject serializedSceneContainer   = new SerializedObject(sceneContainer);

            //Color defaultColor = GUI.backgroundColor;
            //GUI.backgroundColor = color == new Color() ? defaultColor : color;
            EditorGUILayout.BeginHorizontal();
            {
                _isFoldOut[index] = EditorGUILayout.Foldout(
                        _isFoldOut[index],
                        sceneContainer.name
                    );

                if (_isFoldOut[index]) {
                    _popUpOption[index] = EditorGUILayout.Popup(
                        _popUpOption[index],
                        _viewMode,
                        GUILayout.Width(100)
                    );
                }
                
            }
            EditorGUILayout.EndHorizontal();

            if (_isFoldOut[index]) {

                EditorGUI.indentLevel += 1;

                CoreEditorModule.DrawHorizontalLine();

                switch (_popUpOption[index])
                {
                    case 0:
                        _reorderableListOfSceneContainerAsset[index].DoLayoutList();
                        break;

                    case 1:

                        
                        break;
                }

                EditorGUI.indentLevel -= 1;
            }
        }



        #endregion

        #region CustomGUI

        private void HeaderGUI() {

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(_productionSceneContainer == null ? "No Scene Container" : "Active SceneContainer : " + _productionSceneContainer.name, EditorStyles.boldLabel);
 
                if (GUILayout.Button("+SceneContainer")) {

                    AddSceneContainerAsset();
                }
            }
            EditorGUILayout.EndHorizontal();


            CoreEditorModule.DrawHorizontalLine();
            
        }

        private void DrawSceneContainerAddedInBuildSettingsGUI()
        {

            if (_productionSceneContainer != null)
                DrawSceneContainer(_productionSceneIndex, ref _productionSceneContainer);
        }

        private void DrawSceneContainerOtherThanBuildSettingsGUI()
        {
            for (int i = 0; i < _numberOfSceneContainerAsset; i++)
            {

                if (_listOfSceneContainerAsset[i] != _productionSceneContainer)
                {

                    SceneContainerAsset sceneContainer = _listOfSceneContainerAsset[i];
                    DrawSceneContainer(i, ref sceneContainer, new Color(0, 1, 1, 0.1f));
                }
            }
        }

        #endregion

        #region EditorWindow

        [MenuItem("FAITH/Core/SceneManagement/ControlPanel", priority = 0)]
        public static void ShowWindow()
        {

            UpdateListOfSceneContainerAsset();

            EditorWindow = GetWindow<SceneManagementEditorWindow>("SceneManagementPanel", typeof(SceneManagementEditorWindow));

            EditorWindow.minSize = new Vector2(450f, 240f);
            EditorWindow.Show();
        }

        [MenuItem("FAITH/Core/SceneManagement/Push ProductionScene to Build", priority = 0)]
        public static void PushProductionSceneAssetToBuildSettings() {


        }

        public override void OnEnable()
        {
            base.OnEnable();

            WhiteBackgroundGUIStyle = new GUIStyle { normal = { background = Texture2D.whiteTexture } };

            UpdateListOfSceneContainerAsset();
        }

        public void OnGUI()
        {
            HeaderGUI();

            DrawSceneContainerAddedInBuildSettingsGUI();

            DrawSceneContainerOtherThanBuildSettingsGUI();
        }

        #endregion
    }
}

