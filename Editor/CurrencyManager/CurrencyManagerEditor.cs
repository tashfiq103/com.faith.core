namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(CurrencyManager))]
    public class CurrencyManagerEditor : BaseEditorClass
    {
        private CurrencyManager _reference;

        private SerializedProperty _sp_instanceBehaviour;
        private SerializedProperty _sp_accountManagerSettings;

        private Editor _editorForAccountManagerSettings;

        public override void OnEnable()
        {
            base.OnEnable();

            _reference = (CurrencyManager)target;

            if (_reference == null)
                return;

            _sp_instanceBehaviour = serializedObject.FindProperty("instanceBehaviour");
            _sp_accountManagerSettings = serializedObject.FindProperty("currencyManagerSettings");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_sp_instanceBehaviour);
            EditorGUILayout.PropertyField(_sp_accountManagerSettings);

            if (_reference.currencyManagerSettings != null)
            {
                DrawSettingsEditor(_reference.currencyManagerSettings, null, ref _reference.showAccountManagerSettings, ref _editorForAccountManagerSettings);

                if (EditorApplication.isPlaying)
                {
                    DrawHorizontalLine();
                    int numberOfCurrency = _reference.currencyManagerSettings.GetNumberOfAvailableCurrency();

                    for (int i = 0; i < numberOfCurrency; i++)
                    {

                        CurrencyTypeEnum currency = (CurrencyTypeEnum)i;

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField(_reference.GetNameOfCurrency(currency) + " : " + _reference.GetCurrentBalance(currency));
                            if (GUILayout.Button("+1000", GUILayout.Width(100)))
                            {

                                _reference.AddBalance(1000, currency);
                            }

                            if (GUILayout.Button("-1000", GUILayout.Width(100)))
                            {
                                _reference.DeductBalance(1000, currency);
                            }
                        }
                        EditorGUILayout.EndHorizontal();


                    }
                }

            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}


