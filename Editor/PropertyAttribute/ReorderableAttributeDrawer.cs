namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    [CustomPropertyDrawer(typeof(ReorderableAttribute))]
    public class ReorderableAttributeDrawer : PropertyDrawer
    {

        #region Private Variables

        private bool         _initialized = false;
        private bool        _isFoldout = false;
        private int         _popUpValue = 0;
        private string[]    _popupOptions = new string[] { "Generic", "Reorderable" };

        #endregion



        public override void OnGUI(Rect position, SerializedProperty _sourceProperty, GUIContent label)
        {
            if (!_initialized) {

                _initialized = true;
            }

            if (_sourceProperty.displayName == "Element 0") {

                float singleLineHeight = EditorGUIUtility.singleLineHeight;
                SerializedObject _serializedObject  = _sourceProperty.serializedObject;
                ReorderableList _reorderableList    = new ReorderableList(_serializedObject, _sourceProperty)
                {
                    displayAdd = true,
                    displayRemove = true,
                    draggable = true,
                    drawHeaderCallback = rect =>
                    {
                        _isFoldout = EditorGUI.Foldout(
                                new Rect(rect.x, rect.y, rect.width - 125, singleLineHeight),
                                _isFoldout,
                                _sourceProperty.displayName,
                                true
                            );

                        _popUpValue = EditorGUI.Popup(
                            new Rect(rect.x + rect.width - 125, rect.y, 125, singleLineHeight),
                            _popUpValue,
                            _popupOptions);
                    },
                    drawElementCallback = (rect, index, isActive, isFocused) => {

                        SerializedProperty element = _sourceProperty.GetArrayElementAtIndex(index);
                        float heightOfElement = EditorGUI.GetPropertyHeight(element);

                        if (_isFoldout)
                        {

                            EditorGUI.PropertyField(
                                new Rect(rect.x, rect.y, rect.width, heightOfElement),
                                element,
                                true
                            );
                        }
                    },
                    elementHeightCallback = index => {
                        return _isFoldout ? EditorGUI.GetPropertyHeight(_sourceProperty.GetArrayElementAtIndex(index)) : 0;
                    }
                };

                position.height = EditorGUI.GetPropertyHeight(_sourceProperty);
                _reorderableList.DoList(position);
            }
        }
    }
}

