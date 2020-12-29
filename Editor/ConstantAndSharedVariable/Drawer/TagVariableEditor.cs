namespace com.faith.core
{
    using UnityEditor;
    using UnityEditorInternal;

    [CustomEditor(typeof(TagVariable))]
    public class TagVariableEditor : BaseEditorClass
    {
        #region Private Variables

        private TagVariable _reference;

        private string[] _tagsLabel;

        private SerializedProperty _DeveloperDescription;
        private SerializedProperty _tagIndex;
        private SerializedProperty _Value;
        

        #endregion

        #region Editor

        public override void OnEnable()
        {
            base.OnEnable();

            _reference = (TagVariable) target;

            if (_reference == null)
                return;

            _DeveloperDescription = serializedObject.FindProperty("DeveloperDescription");
            _tagIndex = serializedObject.FindProperty("_tagIndex");
            _Value = serializedObject.FindProperty("Value");

            _tagsLabel = InternalEditorUtility.tags;
        }

        public override void OnInspectorGUI()
        {
            CoreEditorModule.ShowScriptReference(serializedObject);

            serializedObject.Update();

            EditorGUILayout.PropertyField(_DeveloperDescription);

            EditorGUI.BeginChangeCheck();
            _tagIndex.intValue = EditorGUILayout.Popup(
                EditorGUIUtility.TrTextContent("Tag", "Select your tag"),
                _tagIndex.intValue,
                _tagsLabel);
            if (EditorGUI.EndChangeCheck()) {

                _tagIndex.serializedObject.ApplyModifiedProperties();

                _Value.stringValue = _tagsLabel[_tagIndex.intValue];
                _Value.serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}

