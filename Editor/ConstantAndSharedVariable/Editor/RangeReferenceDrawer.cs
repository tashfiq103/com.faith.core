namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(RangeReference))]
    public class RangeReferenceDrawer : PropertyDrawer
    {
        private readonly string[] popupOptions =
            { "Use Constant", "Use Variable" };

        private GUIStyle popupStyle;

        private Rect[] SplitedRect(Rect rectToSplit, int n)
        {

            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++)
            {

                rects[i] = new Rect(
                    rectToSplit.position.x + (i * rectToSplit.width / n),
                    rectToSplit.position.y,
                    rectToSplit.width / n,
                    rectToSplit.height);
            }

            float padding = rects[0].width - 40;
            float space = 5;
            rects[0].width -= padding + space;
            rects[2].width -= padding + space;

            rects[1].x -= padding;
            rects[1].width += padding * 2;

            rects[2].x += padding + space;

            return rects;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            // Get properties
            SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
            SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");

            SerializedProperty variable = property.FindPropertyRelative("Variable");
            SerializedProperty variableValue = variable.FindPropertyRelative("Value");

            SerializedProperty min = property.FindPropertyRelative("min");
            SerializedProperty max = property.FindPropertyRelative("max");

            string labelFormat = string.Format(
                    "{0} [ {1} <-> {2} ]",
                    property.displayName,
                    useConstant.boolValue || variable.objectReferenceValue == null ? constantValue.vector2Value.x.ToString("F2") : variableValue.vector2Value.x.ToString("F2"),
                    useConstant.boolValue || variable.objectReferenceValue == null ? constantValue.vector2Value.y.ToString("F2") : variableValue.vector2Value.y.ToString("F2")
                );

            label.text = labelFormat;
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, popupStyle);

            useConstant.boolValue = result == 0;

            if (useConstant.boolValue)
            {

                Rect[] splitedRect = SplitedRect(position, 3);
                min.floatValue = EditorGUI.FloatField(
                        splitedRect[0],
                        min.floatValue
                    );
                max.floatValue = EditorGUI.FloatField(
                        splitedRect[2],
                        max.floatValue
                    );

                float minValue = constantValue.vector2Value.x;
                float maxValue =  constantValue.vector2Value.y;
                EditorGUI.MinMaxSlider(
                        splitedRect[1],
                        ref minValue,
                        ref maxValue,
                        min.floatValue,
                        max.floatValue
                    );

                if (minValue < min.floatValue)
                    minValue = min.floatValue;

                if (maxValue > max.floatValue)
                    maxValue = max.floatValue;

                constantValue.vector2Value = new Vector2(minValue, maxValue);
            }
            else {

                EditorGUI.PropertyField(
                        position,
                        variable,
                        GUIContent.none
                    );
            }
                
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}

