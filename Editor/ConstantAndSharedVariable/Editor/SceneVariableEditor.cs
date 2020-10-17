namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(SceneVariable))]
    public class SceneVariableEditor : BaseEditorClass
    {
        #region Private Variables

        private SceneVariable _reference;

        private SerializedProperty scenePath;
        private SerializedProperty sceneName;

        #endregion

        #region Editor

        public override void OnEnable()
        {
            base.OnEnable();
            _reference = (SceneVariable)target;

            if (_reference == null)
                return;

            scenePath = serializedObject.FindProperty("scenePath");
            sceneName = serializedObject.FindProperty("sceneName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath.stringValue);
            EditorGUI.BeginChangeCheck();
            SceneAsset newScene = EditorGUILayout.ObjectField(
                    "Scene",
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
                scenePath.serializedObject.ApplyModifiedProperties();

                sceneName.stringValue = splitedByDot[0];
                sceneName.serializedObject.ApplyModifiedProperties();

            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}


