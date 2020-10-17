namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        /// <summary>
        /// Options to display in the popup to select constant or variable.
        /// </summary>
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
            SerializedProperty scenePath = property.FindPropertyRelative("scenePath");
            SerializedProperty sceneName = property.FindPropertyRelative("sceneName");
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

            //EditorGUI.PropertyField(position,
            //    useConstant.boolValue ? sceneName : variable,
            //    GUIContent.none);

            if (useConstant.boolValue)
            {

                SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath.stringValue);
                EditorGUI.BeginChangeCheck();
                SceneAsset newScene = EditorGUI.ObjectField(
                        position,
                        "",
                        oldScene,
                        typeof(SceneAsset),
                        false
                    ) as SceneAsset;
                if (EditorGUI.EndChangeCheck())
                {

                    string newPath = AssetDatabase.GetAssetPath(newScene);
                    string[] splitedByDash = newPath.Split('/');
                    string[] splitedByDot = splitedByDash[splitedByDash.Length - 1].Split('.');

                    scenePath.stringValue = newPath;
                    sceneName.stringValue = splitedByDot[0];

                    property.serializedObject.ApplyModifiedProperties();
                }

                if (EditorGUI.EndChangeCheck())
                    property.serializedObject.ApplyModifiedProperties();
            }
            else {

                EditorGUI.PropertyField(position,
                    variable,
                    GUIContent.none);
            }



            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}

//SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets");

//EditorGUI.BeginChangeCheck();
//SceneAsset newScene = EditorGUI.ObjectField(
//        position,
//        "",
//        oldScene,
//        typeof(SceneAsset),
//        false
//    ) as SceneAsset;
//if (EditorGUI.EndChangeCheck())
//{

//    string newPath = AssetDatabase.GetAssetPath(newScene);
//    string[] splitedByDash = newPath.Split('/');
//    string[] splitedByDot = splitedByDash[splitedByDash.Length - 1].Split('.');

//    scenePath.stringValue = newPath;
//    sceneName.stringValue = splitedByDot[0];

//    property.serializedObject.ApplyModifiedProperties();
//}

