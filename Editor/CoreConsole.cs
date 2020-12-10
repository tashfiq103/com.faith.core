namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using System.Collections.Generic;
    

    public class CoreConsole : BaseEditorWindowClass, IPreprocessBuildWithReport
    {

        #region Public Variables

        public int callbackOrder => throw new System.NotImplementedException();

        #endregion

        #region Private Variables

        private static List<CoreConsole> _listOfCoreConsole = new List<CoreConsole>();
        private static List<GameConfiguratorAsset> _listOfGameConfiguretorAsset;

        private CoreConsole _editorWindowOfCoreConsole;

        private bool _isClearOnEnteringPlayMode { get { return _clearOptionStatus[0]; } }
        private bool _isClearnOnBuild { get { return _clearOptionStatus[1]; } } 

        private bool[] _clearOptionStatus= new bool[] { true, false };
        private string[] _clearOptionLable = new string[] { "Clear on Play", "Clear on Build" };


        #endregion

        #region Editor  :   Static

        [MenuItem("FAITH/Core/Core Console", priority = 3)]
        public static void ShowWindow() {

            if (_listOfCoreConsole == null)
                _listOfCoreConsole = new List<CoreConsole>();

            CoreConsole newCoreConsole = CreateInstance<CoreConsole>();
            newCoreConsole.CreateCoreConsole();
        }

        

        #endregion

        #region Editor, Interface

        public override void OnEnable()
        {
            base.OnEnable();

            UpdateGameConfiguretorAsset();
        }

        public void OnGUI()
        {
            HeaderGUI();
        }

        public void OnDestroy()
        {
            _listOfCoreConsole.Remove(this);
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if(_isClearOnEnteringPlayMode)
                ClearAllLog();
        }

        #endregion

        #region Configuretion

        private void ClearAllLog() {

            foreach (GameConfiguratorAsset gameConfiguratorAsset in _listOfGameConfiguretorAsset)
                gameConfiguratorAsset.ClearAllLog();
        }

        private void LogPlayModeState(PlayModeStateChange state)
        {

            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:

                    if (_isClearOnEnteringPlayMode)
                        ClearAllLog();

                    break;
            }
        }


        private void UpdateGameConfiguretorAsset() {

            _listOfGameConfiguretorAsset = CoreEditorModule.GetAsset<GameConfiguratorAsset>();
        }

        private void CreateCoreConsole() {

            EditorApplication.playModeStateChanged += LogPlayModeState;

            UpdateGameConfiguretorAsset();

            string title = "Core Console";

            _editorWindowOfCoreConsole = GetWindow<CoreConsole>(title, typeof(CoreConsole));

            _editorWindowOfCoreConsole.titleContent.text = title;
            _editorWindowOfCoreConsole.minSize = new Vector2(450f, 240f);
            _editorWindowOfCoreConsole.Show();

        }



        #endregion

        #region CustomGUI

        private void HeaderGUI()
        {

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear", GUILayout.Width(50f)))
                {
                    ClearAllLog();
                }
                if (GUILayout.Button("_", GUILayout.Width(10))) {

                    GenericMenu genericMenuForClearMode = new GenericMenu();

                    int numberOfOption = _clearOptionLable.Length;
                    for (int i = 0; i < numberOfOption; i++) {

                        genericMenuForClearMode.AddItem(
                            new GUIContent(_clearOptionLable[i]),
                            _clearOptionStatus[i],
                            (index) => {
                                int selectedIndex = (int) index;   
                                _clearOptionStatus[selectedIndex] = !_clearOptionStatus[selectedIndex];
                            },
                            i);
                    }
                    genericMenuForClearMode.ShowAsContext();
                }
                
            }

            EditorGUILayout.EndHorizontal();
        }

        

        #endregion
    }
}
