namespace com.faith.core
{
    using UnityEditor;
    using UnityEditorInternal;

    [CustomEditor(typeof(LayerVariable))]
    public class LayerVariableEditor : BaseEditorClass
    {
        #region Private Variables

        private LayerVariable _reference;

        private string[] _layersLabel;

        private SerializedProperty _DeveloperDescription;
        private SerializedProperty _layerIndex;
        private SerializedProperty _layerName;


        #endregion

        #region Editor

        public override void OnEnable()
        {
            base.OnEnable();

            _reference = (LayerVariable)target;

            if (_reference == null)
                return;

            _DeveloperDescription = serializedObject.FindProperty("DeveloperDescription");
            _layerIndex = serializedObject.FindProperty("_layerIndex");
            _layerName = serializedObject.FindProperty("_layerName");

            _layersLabel = InternalEditorUtility.layers;
        }

        public override void OnInspectorGUI()
        {
            CoreEditorModule.ShowScriptReference(serializedObject);

            serializedObject.Update();

            EditorGUILayout.PropertyField(_DeveloperDescription);

            EditorGUI.BeginChangeCheck();
            _layerIndex.intValue = EditorGUILayout.Popup(
                EditorGUIUtility.TrTextContent("Layer", "Select your layer. You can only pick one from the following option"),
                _layerIndex.intValue,
                _layersLabel);
            if (EditorGUI.EndChangeCheck())
            {

                _layerIndex.serializedObject.ApplyModifiedProperties();

                _layerName.stringValue = _layersLabel[_layerIndex.intValue];
                _layerName.serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion


    }
}

