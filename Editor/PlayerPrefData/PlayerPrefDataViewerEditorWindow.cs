namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    public class PlayerPrefDataViewerEditorWindow :   BaseEditorWindowClass
    {

        private static PlayerPrefDataViewerEditorWindow EditorWindow;

        private static Vector2 m_ScrollPosition;
        private static string m_SearchText;

        private const float widthForDataType = 100;
        private const float widthForValue = 140;
        private const float widthForValueChange = 140;
        

        #region EditorWindow

        [MenuItem("FAITH/Data/PlayerPrefs DataViwer")]
        public static void ShowWindow()
        {

            EditorWindow = GetWindow<PlayerPrefDataViewerEditorWindow>("PlayerPrefs DataViwer", typeof(PlayerPrefDataViewerEditorWindow));

            EditorWindow.minSize = new Vector2(450f, 240f);
            EditorWindow.Show();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        private void OnGUI()
        {

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                m_SearchText = EditorGUILayout.TextField(
                        "Search",
                        m_SearchText
                    );
            }
            EditorGUILayout.EndHorizontal();
            

            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition, false, false);
            {
                //SettingMenu
                DrawHorizontalLine();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("PrefKeys (" + PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData.Count.ToString() + ")", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("DataType", EditorStyles.boldLabel, GUILayout.Width(widthForDataType));
                    EditorGUILayout.LabelField("Value", EditorStyles.boldLabel, GUILayout.Width(widthForValue));
                    EditorGUILayout.LabelField("SetValue", EditorStyles.boldLabel, GUILayout.Width(widthForValueChange));
                }
                EditorGUILayout.EndHorizontal();
                DrawHorizontalLine();

                //Traversing List
                int t_Index = 1;
                foreach (PlayerPrefDataSettings.PlayerPrefEditorData t_PlayerPrefDataKey in PlayerPrefDataSettings.listOfUsedPlayerPrefEditorData) {

                    if (m_SearchText == null || (m_SearchText != null && (m_SearchText.Length == 0 || (m_SearchText.Length > 0 && t_PlayerPrefDataKey.key.Contains(m_SearchText))))) {


                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField(t_Index + " : " + t_PlayerPrefDataKey.key);

                            if (t_PlayerPrefDataKey.type == typeof(bool))
                            {

                                EditorGUILayout.LabelField("BOOLEAN", GUILayout.Width(widthForDataType));
                                EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(widthForValue));

                                bool t_BoolValue = (bool)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(bool));
                                EditorGUI.BeginChangeCheck();
                                {
                                    t_BoolValue = EditorGUILayout.Toggle(
                                        "",
                                        t_BoolValue,
                                        GUILayout.Width(widthForValueChange)
                                    );
                                }
                                if (EditorGUI.EndChangeCheck())
                                {
                                    PlayerPrefDataSettings.SetData<bool>(t_PlayerPrefDataKey.key, t_BoolValue.ToString());
                                }
                            }
                            else if (t_PlayerPrefDataKey.type == typeof(int))
                            {

                                EditorGUILayout.LabelField("INTEGER", GUILayout.Width(widthForDataType));
                                EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(widthForValue));

                                int t_IntValue = (int)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(int));

                                EditorGUI.BeginChangeCheck();
                                {
                                    t_IntValue = EditorGUILayout.IntField(
                                        "",
                                        t_IntValue,
                                        GUILayout.Width(widthForValueChange)
                                    );
                                }
                                if (EditorGUI.EndChangeCheck())
                                {
                                    PlayerPrefDataSettings.SetData<int>(t_PlayerPrefDataKey.key, t_IntValue.ToString());
                                }
                            }
                            else if (t_PlayerPrefDataKey.type == typeof(float))
                            {
                                EditorGUILayout.LabelField("FLOAT", GUILayout.Width(widthForDataType));
                                EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(widthForValue));

                                float t_FloatValue = (float)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(float));

                                EditorGUI.BeginChangeCheck();
                                {
                                    t_FloatValue = EditorGUILayout.FloatField(
                                        "",
                                        t_FloatValue,
                                        GUILayout.Width(widthForValueChange)
                                    );
                                }
                                if (EditorGUI.EndChangeCheck())
                                {
                                    PlayerPrefDataSettings.SetData<float>(t_PlayerPrefDataKey.key, t_FloatValue.ToString());
                                }

                            }
                            else if (t_PlayerPrefDataKey.type == typeof(double))
                            {
                                EditorGUILayout.LabelField("DOUBLE", GUILayout.Width(widthForDataType));
                                EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(widthForValue));

                                double t_DoubleValue = (double)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(double));

                                EditorGUI.BeginChangeCheck();
                                {
                                    t_DoubleValue = EditorGUILayout.DoubleField(
                                        "",
                                        t_DoubleValue,
                                        GUILayout.Width(widthForValueChange)
                                    );
                                }
                                if (EditorGUI.EndChangeCheck())
                                {
                                    PlayerPrefDataSettings.SetData<double>(t_PlayerPrefDataKey.key, t_DoubleValue.ToString());
                                }

                            }
                            else if (t_PlayerPrefDataKey.type == typeof(string))
                            {

                                EditorGUILayout.LabelField("STRING", GUILayout.Width(widthForDataType));
                                EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(widthForValue));

                                string t_StringValue = (string)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(string));

                                EditorGUI.BeginChangeCheck();
                                {
                                    t_StringValue = EditorGUILayout.TextField(
                                        "",
                                        t_StringValue,
                                        GUILayout.Width(widthForValueChange)
                                    );
                                }
                                if (EditorGUI.EndChangeCheck())
                                {
                                    PlayerPrefDataSettings.SetData<string>(t_PlayerPrefDataKey.key, t_StringValue.ToString());
                                }
                            }
                            else if (t_PlayerPrefDataKey.type == typeof(System.DateTime))
                            {

                                EditorGUILayout.LabelField("DATE_TIME", GUILayout.Width(widthForDataType));
                                EditorGUILayout.LabelField(t_PlayerPrefDataKey.value, GUILayout.Width(widthForValue));

                                System.DateTime dateTimeValue = (System.DateTime)System.Convert.ChangeType(t_PlayerPrefDataKey.value, typeof(System.DateTime));

                                EditorGUI.BeginChangeCheck();
                                {
                                    GUI.enabled = false;
                                    EditorGUILayout.TextField(
                                        "",
                                        dateTimeValue.ToString(),
                                        GUILayout.Width(widthForValueChange)
                                    );
                                    GUI.enabled = true;
                                }
                                if (EditorGUI.EndChangeCheck())
                                {
                                    PlayerPrefDataSettings.SetData<System.DateTime>(t_PlayerPrefDataKey.key, dateTimeValue.ToString());
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                    }

                    t_Index++;
                }
            }
            EditorGUILayout.EndScrollView();
        }

        #endregion


        #region PublicCallback

        

        #endregion
    }
}

