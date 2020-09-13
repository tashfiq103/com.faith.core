namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(AccountManager))]
    public class AccountManagerEditor : BaseEditorClass
    {
        private AccountManager _reference;

        private SerializedProperty _spDurationForAnimation;
        private SerializedProperty _spAnimationCurve;

        private void OnEnable()
        {
            _reference = (AccountManager)target;

            if (_reference == null)
                return;

            _spDurationForAnimation = serializedObject.FindProperty("durationForAnimation");
            _spAnimationCurve = serializedObject.FindProperty("animationCurve");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_spDurationForAnimation);
            EditorGUILayout.PropertyField(_spAnimationCurve);

            if (EditorApplication.isPlaying)
            {

                int numberOfCurrency = _reference.GetNumberOfAvailableCurrency();

                for (int i = 0; i < numberOfCurrency; i++)
                {

                    CURRENCY currency = (CURRENCY)i;

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

            serializedObject.ApplyModifiedProperties();
        }
    }
}


