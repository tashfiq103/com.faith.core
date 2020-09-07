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

                //SettingMenu
                DrawHorizontalLine();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("PrefKeys", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("DataType", EditorStyles.boldLabel, GUILayout.Width(100));
                    EditorGUILayout.LabelField("Value", EditorStyles.boldLabel, GUILayout.Width(100));
                    EditorGUILayout.LabelField("SetValue", EditorStyles.boldLabel, GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
                DrawHorizontalLine();

                //Traversing List
                foreach (PlayerPrefDataSettings.PlayerPrefEditorData t_PlayerPrefDataKey in PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData) {

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(t_Index + " : " + t_PlayerPrefDataKey.key);

                        if (t_PlayerPrefDataKey.type == typeof(bool)) {

                            EditorGUILayout.LabelField("BOOLEAN", GUILayout.Width(100));
                            EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(100));

                            bool t_BoolValue = (bool)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(bool));
                            EditorGUI.BeginChangeCheck();
                            {
                                t_BoolValue = EditorGUILayout.Toggle(
                                    "",
                                    t_BoolValue,
                                    GUILayout.Width(100)
                                );
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                PlayerPrefDataSettings.SetData(t_PlayerPrefDataKey.key, t_PlayerPrefDataKey.type, t_BoolValue.ToString());
                            }
                        }
                        else if (t_PlayerPrefDataKey.type == typeof(int)){

                            EditorGUILayout.LabelField("INTEGER", GUILayout.Width(100));
                            EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(100));

                            int t_IntValue = (int)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(int));

                            EditorGUI.BeginChangeCheck();
                            {
                                t_IntValue = EditorGUILayout.IntField(
                                    "",
                                    t_IntValue,
                                    GUILayout.Width(100)
                                );
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                PlayerPrefDataSettings.SetData(t_PlayerPrefDataKey.key, t_PlayerPrefDataKey.type, t_IntValue.ToString());
                            }
                        }
                        else if (t_PlayerPrefDataKey.type == typeof(float))
                        {
                            EditorGUILayout.LabelField("FLOAT", GUILayout.Width(100));
                            EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(100));

                            float t_FloatValue = (float)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(float));

                            EditorGUI.BeginChangeCheck();
                            {
                                t_FloatValue = EditorGUILayout.FloatField(
                                    "",
                                    t_FloatValue,
                                    GUILayout.Width(100)
                                );
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                PlayerPrefDataSettings.SetData(t_PlayerPrefDataKey.key, t_PlayerPrefDataKey.type, t_FloatValue.ToString());
                            }

                        }
                        else if (t_PlayerPrefDataKey.type == typeof(double))
                        {
                            EditorGUILayout.LabelField("DOUBLE", GUILayout.Width(100));
                            EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(100));

                            double t_DoubleValue = (double)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(double));

                            EditorGUI.BeginChangeCheck();
                            {
                                t_DoubleValue = EditorGUILayout.DoubleField(
                                    "",
                                    t_DoubleValue,
                                    GUILayout.Width(100)
                                );
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                PlayerPrefDataSettings.SetData(t_PlayerPrefDataKey.key, t_PlayerPrefDataKey.type, t_DoubleValue.ToString());
                            }

                        }
                        else if (t_PlayerPrefDataKey.type == typeof(string))
                        {

                            EditorGUILayout.LabelField("STRING", GUILayout.Width(100));
                            EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(100));

                            string t_StringValue = (string)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(string));

                            EditorGUI.BeginChangeCheck();
                            {
                                t_StringValue = EditorGUILayout.TextField(
                                    "",
                                    t_StringValue,
                                    GUILayout.Width(100)
                                );
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                PlayerPrefDataSettings.SetData(t_PlayerPrefDataKey.key, t_PlayerPrefDataKey.type, t_StringValue.ToString());
                            }
                        }
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

