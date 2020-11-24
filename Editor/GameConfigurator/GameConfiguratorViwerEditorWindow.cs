namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Collections.Generic;

    public class GameConfiguratorViwerEditorWindow : BaseEditorWindowClass
    {
        #region Private Variables

        private static GameConfiguratorViwerEditorWindow EditorWindow;

        private static GameConfiguratorAsset        _productionGameConfiguretorAsset;
        private static List<GameConfiguratorAsset> _listOfGameConfiguretorAsset;
        private static Editor[] _editorForGameConfiguretorAsset;
        private static int _numberOfGameConfiguretorAsset;
        private static bool[]   _isFoldOut;
        private static bool _isFoldOutOtherConfigAsset;
        

        private static Vector2 _scrollPosition;

        private const string _defaultName               = "NewGameConfig";
        private static string _nameOfConfiguretorFile   = _defaultName;

        private static GUIStyle DefaultStyle = new GUIStyle();
        private static GUIStyle HeighlightedBackgroundStyle = new GUIStyle();
        private static GUIStyle HeighlightedBackgroundWithBoldStyle = new GUIStyle();

        #endregion

        #region Configuretion

        private static int IsThereAnyGameConfigAssetWithTheGivenName(string name)
        {
            int _numberOfDuplicateName = 0;
            List<GameConfiguratorAsset> gameConfiguratorAssets = GetAsset<GameConfiguratorAsset>();
            foreach (GameConfiguratorAsset gameConfigAsset in gameConfiguratorAssets)
            {
                if (gameConfigAsset.name.Contains(name))
                    _numberOfDuplicateName++;
            }

            return _numberOfDuplicateName;
        }

        private static bool IsAnyGameConfiguretionAssetUsedByGameConfiguretionManager() {

            List<GameConfiguratorAsset> gameConfiguratorAssets = GetAsset<GameConfiguratorAsset>();
            foreach (GameConfiguratorAsset gameConfigAsset in gameConfiguratorAssets) {

                if (gameConfigAsset.EditorAccessIfUsedByCentralGameConfiguretion)
                    return true;
            }
            CoreDebugger.Debug.LogError("Please assign any of your 'GameConfiguretionAsset' to 'GameConfiguretionManager'", prefix : "GameConfiguretorAsset");
            return false;
        }

        private static void SetLinkStatusWithCentralGameConfiguretion(bool statusFlag) {

            if (IsAnyGameConfiguretionAssetUsedByGameConfiguretionManager()) {

                List<GameConfiguratorAsset> gameConfiguratorAssets = GetAsset<GameConfiguratorAsset>();

                foreach (GameConfiguratorAsset gameConfigAsset in gameConfiguratorAssets)
                {
                    if (!gameConfigAsset.EditorAccessIfUsedByCentralGameConfiguretion)
                    {
                        SerializedObject serializedGameConfiguretorAsset = new SerializedObject(gameConfigAsset);

                        SerializedProperty _linkWithCentralGameConfiguretion = serializedGameConfiguretorAsset.FindProperty("_linkWithCentralGameConfiguretion");
                        _linkWithCentralGameConfiguretion.boolValue = statusFlag;
                        _linkWithCentralGameConfiguretion.serializedObject.ApplyModifiedProperties();

                        serializedGameConfiguretorAsset.ApplyModifiedProperties();
                    }
                }
            }
        }

        private static void UpdateListOfGameConfiguretorAsset() {

            _listOfGameConfiguretorAsset = GetAsset<GameConfiguratorAsset>();
            _numberOfGameConfiguretorAsset = _listOfGameConfiguretorAsset.Count;

            //Marking   :   Central Game Configuretor Asset
            if (_productionGameConfiguretorAsset == null) {

                foreach (GameConfiguratorAsset gameConfiguratorAsset in _listOfGameConfiguretorAsset)
                {
                    if (gameConfiguratorAsset.EditorAccessIfUsedByCentralGameConfiguretion)
                        _productionGameConfiguretorAsset = gameConfiguratorAsset;
                }
            }

            foreach (GameConfiguratorAsset gameConfiguratorAsset in _listOfGameConfiguretorAsset)
            {
                //if : It is the central game configuretor asset but not matched with the cashed production asset. Remove It From Prodcution
                if (gameConfiguratorAsset.EditorAccessIfUsedByCentralGameConfiguretion && _productionGameConfiguretorAsset != gameConfiguratorAsset) {

                    SerializedObject serializedGameConfiguretorAsset = new SerializedObject(gameConfiguratorAsset);

                    SerializedProperty _isUsedByCentralGameConfiguretion = serializedGameConfiguretorAsset.FindProperty("_isUsedByCentralGameConfiguretion");
                    _isUsedByCentralGameConfiguretion.boolValue = false;
                    _isUsedByCentralGameConfiguretion.serializedObject.ApplyModifiedProperties();

                    serializedGameConfiguretorAsset.ApplyModifiedProperties();
                }
            }

            _isFoldOut                      = new bool[_numberOfGameConfiguretorAsset];
            _editorForGameConfiguretorAsset = new Editor[_numberOfGameConfiguretorAsset];
        }

        private void CreateNewGameConfiguretorAsset() {

            if (!Directory.Exists(CoreConstant.DirectoryForGameConfiguretionAsset))
                Directory.CreateDirectory(CoreConstant.DirectoryForGameConfiguretionAsset);

            _nameOfConfiguretorFile     = _nameOfConfiguretorFile.Length == 0 ? _defaultName : _nameOfConfiguretorFile;
            int numberOfDuplicateName   = IsThereAnyGameConfigAssetWithTheGivenName(_nameOfConfiguretorFile);
            string absoluteName         = _nameOfConfiguretorFile + (numberOfDuplicateName == 0 ? "" : (" " + numberOfDuplicateName));

            GameConfiguratorAsset newGameConfiguretionAsset = ScriptableObject.CreateInstance<GameConfiguratorAsset>();

            AssetDatabase.CreateAsset(newGameConfiguretionAsset, CoreConstant.DirectoryForGameConfiguretionAsset + "/" + absoluteName + ".asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = newGameConfiguretionAsset;

            UpdateListOfGameConfiguretorAsset();
        }

        #endregion

        #region EditorWindow

        [MenuItem("FAITH/Core/GameConfigurator/ControlPanel", priority = 0)]
        public static void ShowWindow()
        {

            UpdateListOfGameConfiguretorAsset();

            EditorWindow = GetWindow<GameConfiguratorViwerEditorWindow>("ControlPanel", typeof(GameConfiguratorViwerEditorWindow));

            EditorWindow.minSize = new Vector2(450f, 240f);
            EditorWindow.Show();
        }

        [MenuItem("FAITH/Core/GameConfigurator/Use Production Settings", priority = 1)]
        public static void LinkWithProductionGameConfiguretor() {

            SetLinkStatusWithCentralGameConfiguretion(true);
        }

        [MenuItem("FAITH/Core/GameConfigurator/Use Standalone Settings", priority = 1)]
        public static void UnlinkWithProductionGameConfiguretor()
        {
            SetLinkStatusWithCentralGameConfiguretion(false);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            HeighlightedBackgroundStyle         = new GUIStyle { normal = { background = Texture2D.whiteTexture } };
            HeighlightedBackgroundWithBoldStyle = new GUIStyle { normal = { background = Texture2D.whiteTexture }, fontStyle = FontStyle.Bold };
            _nameOfConfiguretorFile     = _defaultName;

            UpdateListOfGameConfiguretorAsset();
        }

        public void OnGUI()
        {

            HeaderGUI();

            if (_productionGameConfiguretorAsset == null)
            {
                EditorGUILayout.HelpBox("Please assign at least one 'GameConfiguretorAsset' to 'GameConfiguretorManager' in order configure through 'ControlPanel'", MessageType.Error);
            }
            else
            {
                GameConfiguretorGUI();
            }
        }

        #endregion

        #region GUI :   Section

        private void HeaderGUI() {

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                _nameOfConfiguretorFile = EditorGUILayout.TextField("Name", _nameOfConfiguretorFile);
                if (GUILayout.Button("+GameConfiguretorAsset", GUILayout.Width(175f))) {

                    CreateNewGameConfiguretorAsset();
                }

                if (GUILayout.Button("Refresh", GUILayout.Width(100f)))
                {

                    UpdateListOfGameConfiguretorAsset();
                }
            }
            EditorGUILayout.EndHorizontal();

            DrawHorizontalLine();
        }

        private void GameConfiguretorGUI() {

            Color defaultBackgroundColor    = GUI.backgroundColor;
            Color defaultContentColor       = GUI.contentColor;

            if (_numberOfGameConfiguretorAsset > 0) {

                GUI.backgroundColor = Color.yellow;
                EditorGUILayout.LabelField("Production Asset", HeighlightedBackgroundWithBoldStyle);
                GUI.backgroundColor = defaultBackgroundColor;

                EditorGUILayout.Space();
                DrawSettingsEditor(_productionGameConfiguretorAsset, null, ref _isFoldOut[0], ref _editorForGameConfiguretorAsset[0]);

                if (_numberOfGameConfiguretorAsset > 1)
                {

                    DrawHorizontalLine();
                    GUI.contentColor = Color.cyan;
                    _isFoldOutOtherConfigAsset = EditorGUILayout.Foldout(
                            _isFoldOutOtherConfigAsset,
                            "Other Asset",
                            true
                        );
                    GUI.contentColor = defaultContentColor;
                }

                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                {
                    for (int i = 1; i < _numberOfGameConfiguretorAsset; i++)
                    {
                        if (_isFoldOutOtherConfigAsset)
                        {
                                if (_productionGameConfiguretorAsset == _listOfGameConfiguretorAsset[i])
                                    DrawSettingsEditor(_listOfGameConfiguretorAsset[0], null, ref _isFoldOut[i], ref _editorForGameConfiguretorAsset[i]);
                                else if (_productionGameConfiguretorAsset != _listOfGameConfiguretorAsset[i])
                                    DrawSettingsEditor(_listOfGameConfiguretorAsset[i], null, ref _isFoldOut[i], ref _editorForGameConfiguretorAsset[i]);




                            if (i < _numberOfGameConfiguretorAsset - 1)
                            {
                                EditorGUILayout.Space();
                                DrawHorizontalLine();
                                EditorGUILayout.Space();
                            }
                        }
                    }

                }
                EditorGUILayout.EndScrollView();
            }
        }
        #endregion
    }
}

