namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    [CustomPropertyDrawer(typeof(LayerReference))]
    public class LayerReferenceDrawer : PropertyDrawer
    {
        private readonly string[] popupOptions =
            { "Use Constant", "Use Variable" };

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
            SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
            SerializedProperty variable = property.FindPropertyRelative("Variable");

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
                SerializedProperty _layerIndex = property.FindPropertyRelative("_layerIndex");
                string[] _layersLabel = InternalEditorUtility.layers;

                EditorGUI.BeginChangeCheck();
                _layerIndex.intValue = EditorGUI.Popup(
                        position,
                        _layerIndex.intValue,
                        _layersLabel
                    );
                if (EditorGUI.EndChangeCheck())
                {
                    _layerIndex.serializedObject.ApplyModifiedProperties();

                    constantValue.stringValue = _layersLabel[_layerIndex.intValue];
                    constantValue.serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {

                EditorGUI.PropertyField(position,
                variable,
                GUIContent.none);
            }



            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}

