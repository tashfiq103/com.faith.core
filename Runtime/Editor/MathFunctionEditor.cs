
namespace com.faith.Math
{

    using UnityEditor;

    [CustomEditor(typeof(MathFunction))]
    public class MathFunctionEditor : Editor
    {

        private MathFunction Reference;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {

            Reference = (MathFunction)target;

            if (MathFunction.Instance == null)
            {

                MathFunction.Instance = Reference;
            }
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();
        }
    }
}


