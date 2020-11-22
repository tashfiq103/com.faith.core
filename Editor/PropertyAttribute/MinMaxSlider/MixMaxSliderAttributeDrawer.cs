namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(MixMaxSliderAttribute))]
    public class MixMaxSliderAttributeDrawer : PropertyDrawer
    {
        #region Configuretion

        private float spaceValue = 5;
        private float paddingValue = 40;


        private Rect[] SplitedRect(Rect rectToSplit, int n) {

            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++) {

                rects[i] = new Rect(
                    rectToSplit.position.x + (i * rectToSplit.width / n),
                    rectToSplit.position.y,
                    rectToSplit.width / n,
                    rectToSplit.height);
            }

            float padding = rects[0].width - paddingValue;

            rects[0].width -= padding + spaceValue;
            rects[2].width -= padding + spaceValue;

            rects[1].x -= padding;
            rects[1].width += padding * 2;

            rects[2].x += padding + spaceValue;

            return rects;
        }

        #endregion

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MixMaxSliderAttribute minMaxAttribute   = (MixMaxSliderAttribute)attribute;
            SerializedPropertyType propertyType     = property.propertyType;

            label.tooltip = minMaxAttribute.min.ToString("F2") + " to " + minMaxAttribute.max.ToString("F2");

            Rect controlRect = EditorGUI.PrefixLabel(position, label);

            Rect[] splitedRect = SplitedRect(controlRect, 3);

            EditorGUI.BeginChangeCheck();
            {

                Vector2 vector = Vector2.zero;

                switch (propertyType)
                {
                    case SerializedPropertyType.Vector2:
                        vector = property.vector2Value;
                        break;
                    case SerializedPropertyType.Vector2Int:
                        vector = property.vector2IntValue;
                        break;
                }

                float minVal = vector.x;
                float maxVal = vector.y;

                minVal = EditorGUI.FloatField(splitedRect[0], float.Parse(minVal.ToString("F2")));
                maxVal = EditorGUI.FloatField(splitedRect[2], float.Parse(maxVal.ToString("F2")));

                EditorGUI.MinMaxSlider(splitedRect[1], ref minVal, ref maxVal, minMaxAttribute.min, minMaxAttribute.max);

                if (minVal < minMaxAttribute.min)
                    minVal = minMaxAttribute.min;

                if (maxVal > minMaxAttribute.max)
                    maxVal = minMaxAttribute.max;

                if (EditorGUI.EndChangeCheck())
                {

                    switch (propertyType)
                    {
                        case SerializedPropertyType.Vector2:
                            property.vector2Value = vector;
                            break;
                        case SerializedPropertyType.Vector2Int:
                            property.vector2IntValue = new Vector2Int((int)vector.x, (int)vector.y);
                            break;
                    }
                }
            }
            

            
        }
    }
}

