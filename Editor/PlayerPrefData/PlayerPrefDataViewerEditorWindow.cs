namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    public class PlayerPrefDataViewerEditorWindow :   BaseEditorWindowClass
    {

        private static PlayerPrefDataViewerEditorWindow EditorWindow;

        private static Vector2 m_ScrollPosition;

        #region EditorWindow

        [MenuItem("FAITH/PlayerPrefData/DataViwer")]
        public static void ShowWindow()
        {

            EditorWindow = GetWindow<PlayerPrefDataViewerEditorWindow>("PlayerPrefData Viwer", typeof(PlayerPrefDataViewerEditorWindow));

            EditorWindow.minSize = new Vector2(360f, 240f);
            EditorWindow.Show();
        }

        private void OnEnable()
        {
            
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("NumberOfPlayerPrefsData" + " : " + PlayerPrefDataSettings.listOfUsedPlayerPrefKey.Count.ToString());
            if (GUILayout.Button("Generate Random PlayerPrefData")) {

                GenerateRandomPlayerPrefs();
            }

            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition, false, false);
            {
                int t_Index = 1;
                foreach (PlayerPrefDataSettings.PlayerPrefEditorData t_PlayerPrefDataKey in PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData) {

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(t_Index + " : " + t_PlayerPrefDataKey.key, EditorStyles.boldLabel);

                        string t_Type = "UNKNOWN";
                        if (t_PlayerPrefDataKey.type == typeof(bool)) {

                            t_Type = "BOOLEAN";
                        }else if (t_PlayerPrefDataKey.type == typeof(int)){

                            t_Type = "INTEGER";
                        }else if (t_PlayerPrefDataKey.type == typeof(float))
                        {

                            t_Type = "FLOAT";
                        }else if (t_PlayerPrefDataKey.type == typeof(double))
                        {

                            t_Type = "DOUBLE";
                        }else if (t_PlayerPrefDataKey.type == typeof(string))
                        {

                            t_Type = "STRING";
                        }


                        EditorGUILayout.LabelField(t_Type, GUILayout.Width(100));
                        EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(125));
                    }
                    EditorGUILayout.EndHorizontal();
                    t_Index++;
                }
            }
            EditorGUILayout.EndScrollView();
        }

        #endregion


        #region PublicCallback

        private void GenerateRandomPlayerPrefs() {

            List<string> t_RandomKeys = new List<string>();
            int t_NumberOfRandomPlayerPrefsData = 100;
            for (int i = 0; i < t_NumberOfRandomPlayerPrefsData; i++) {

                string t_RandomKey = Random.Range(1, 100000).ToString();
                
                if (!t_RandomKeys.Contains(t_RandomKey)) {

                    if (i % 7 == 0)
                    {
                        PlayerPrefData<string> t_RandomPlayerPrefData = new PlayerPrefData<string>("string_" + t_RandomKey, Random.Range(0f, 100000f).ToString());
                    }
                    else if (i % 5 == 0)
                    {
                        PlayerPrefData<double> t_RandomPlayerPrefData = new PlayerPrefData<double>("double_" + t_RandomKey, Random.Range(0f, 100f));
                    }
                    else if (i % 4 == 0)
                    {
                        PlayerPrefData<float> t_RandomPlayerPrefData = new PlayerPrefData<float>("float_" + t_RandomKey, Random.Range(0f, 100f));
                    }
                    else if (i % 3 == 0)
                    {
                        PlayerPrefData<int> t_RandomPlayerPrefData = new PlayerPrefData<int>("int_" + t_RandomKey, Random.Range(0, 100));
                    }
                    else{

                        PlayerPrefData<bool> t_RandomPlayerPrefData = new PlayerPrefData<bool>("bool_" + t_RandomKey, Random.Range(0, 1) == 0 ? false : true);
                    }

                    t_RandomKeys.Add(t_RandomKey);
                    
                }
            }
        }

        #endregion
    }
}

