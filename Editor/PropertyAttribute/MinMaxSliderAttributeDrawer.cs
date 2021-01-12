namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderAttributeDrawer : PropertyDrawer
    {
        #region Configuretion

        private Rect[] SplitedRect(Rect rectToSplit, int n) {

            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++) {

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

        #endregion

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinMaxSliderAttribute minMaxAttribute   = (MinMaxSliderAttribute)attribute;
            SerializedPropertyType propertyType     = property.propertyType;

            label.tooltip = string.Format("{0} to {1} : {2}", minMaxAttribute.min.ToString("F2"), minMaxAttribute.max.ToString("F2"), "It Support Only Vector2 & Vector2Int");

            Rect controlRect = EditorGUI.PrefixLabel(position, label);

            Rect[] splitedRect = SplitedRect(controlRect, 3);

            if (propertyType == SerializedPropertyType.Vector2)
            {
                EditorGUI.BeginChangeCheck();

                Vector2 vector = property.vector2Value;
                float minVal = vector.x;
                float maxVal = vector.y;

                minVal = EditorGUI.FloatField(splitedRect[0], float.Parse(minVal.ToString("F2")));
                maxVal = EditorGUI.FloatField(splitedRect[2], float.Parse(maxVal.ToString("F2")));

                EditorGUI.MinMaxSlider(
                    splitedRect[1],
                    ref minVal,
                    ref maxVal,
                    minMaxAttribute.min,
                    minMaxAttribute.max);

                if (minVal < minMaxAttribute.min)
                    minVal = minMaxAttribute.min;

                if (maxVal > minMaxAttribute.max)
                    maxVal = minMaxAttribute.max;

                vector = new Vector2(minVal > maxVal ? maxVal : minVal, maxVal);

                if (EditorGUI.EndChangeCheck())
                {

                    property.vector2Value = vector;
                }
            }
            else if (propertyType == SerializedPropertyType.Vector2Int)
            {

                EditorGUI.BeginChangeCheck();

                Vector2Int vector = property.vector2IntValue;
                float minVal = vector.x;
                float maxVal = vector.y;

                minVal = EditorGUI.FloatField(splitedRect[0], float.Parse(minVal.ToString("F2")));
                maxVal = EditorGUI.FloatField(splitedRect[2], float.Parse(maxVal.ToString("F2")));

                EditorGUI.MinMaxSlider(
                    splitedRect[1],
                    ref minVal,
                    ref maxVal,
                    minMaxAttribute.min,
                    minMaxAttribute.max);

                if (minVal < minMaxAttribute.min)
                    minVal = minMaxAttribute.min;

                if (maxVal > minMaxAttribute.max)
                    maxVal = minMaxAttribute.max;

                vector = new Vector2Int(Mathf.FloorToInt(minVal > maxVal ? maxVal : minVal), Mathf.FloorToInt(maxVal));

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2IntValue = vector;
                }
            }
            else {

                Debug.LogWarning("MinMaxSlider only support Vector2 & Vector2Int. the following type '" + propertyType + "' is not supported");
            }
        }
    }
}

