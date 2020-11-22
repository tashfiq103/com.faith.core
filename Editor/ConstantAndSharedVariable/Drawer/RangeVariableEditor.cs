namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(RangeVariable))]
    public class RangeVariableEditor    :   BaseEditorClass
    {

        #region Private Variables

        private RangeVariable _reference;

        private SerializedProperty DeveloperDescription;
        private SerializedProperty Value;

        private SerializedProperty min;
        private SerializedProperty max;

        #endregion

        #region Editor

        public override void OnEnable()
        {
            base.OnEnable();

            _reference = (RangeVariable)target;

            if (_reference == null)
                return;

            DeveloperDescription = serializedObject.FindProperty("DeveloperDescription");
            Value = serializedObject.FindProperty("Value");

            min = serializedObject.FindProperty("min");
            max = serializedObject.FindProperty("max");
        }

        public override void OnInspectorGUI()
        {
            ShowScriptReference();

            serializedObject.Update();

            EditorGUILayout.PropertyField(DeveloperDescription);

            DrawHorizontalLine();
            EditorGUILayout.HelpBox("In order get the return type from the following Range. use 'RangeReference' in your script, which will return the value from the given range from the 'ScriptableObject' based on your 'Property' configuretion", MessageType.Info);
            DrawHorizontalLine();

            MinMaxSliderGUI();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Configuretion

        private void MinMaxSliderGUI() {

            string lable = string.Format(
                    "{0} [ {1} <-> {2} ]",
                    "Range",
                    Value.vector2Value.x.ToString("F2"),
                    Value.vector2Value.y.ToString("F2")
                );
            float minValue = Value.vector2Value.x;
            float maxValue = Value.vector2Value.y;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(lable);

                EditorGUI.BeginChangeCheck();
                min.floatValue = EditorGUILayout.FloatField(
                    min.floatValue,
                    GUILayout.Width(40));
                EditorGUILayout.MinMaxSlider(
                        ref minValue,
                        ref maxValue,
                        min.floatValue,
                        max.floatValue
                    );
                max.floatValue = EditorGUILayout.FloatField(
                    max.floatValue,
                    GUILayout.Width(40));
                if (EditorGUI.EndChangeCheck())
                {

                    if (minValue < min.floatValue)
                        minValue = min.floatValue;

                    if (maxValue > max.floatValue)
                        maxValue = max.floatValue;

                    Value.vector2Value = new Vector2(minValue, maxValue);
                }
            }
            EditorGUILayout.EndHorizontal();
            
        }

        #endregion
    }
}

