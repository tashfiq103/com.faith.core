namespace com.faith.core
{
    using UnityEngine;

    [System.Serializable]
    public class RangeReference 
    {
        public bool             UseConstant = true;
        public Vector2          ConstantValue;
        public RangeVariable    Variable;

        [SerializeField] private float min = 0;
        [SerializeField] private float max = 1;

        public RangeReference() {

        }

        public RangeReference(Vector2 value) {

            UseConstant = true;
            ConstantValue = value;

        }

        public float Value
        {
            get
            {
                if (UseConstant)
                    return Random.Range(ConstantValue.x, ConstantValue.y);
                else
                {
                    if (Variable != null)
                        return Random.Range(Variable.Value.x, Variable.Value.y);
                    else
                    {
                        CoreDebugger.Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                        return Random.Range(ConstantValue.x, ConstantValue.y);
                    }
                }
            }
        }

        public float InterpolatedValue(float interpolationPoint) {

            interpolationPoint = Mathf.Clamp01(interpolationPoint);

            if (UseConstant)
                return Mathf.Lerp(ConstantValue.x, ConstantValue.y, interpolationPoint);
            else
            {
                if (Variable != null)
                    return Mathf.Lerp(Variable.Value.x, Variable.Value.y, interpolationPoint);
                else
                {
                    CoreDebugger.Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                    return Mathf.Lerp(ConstantValue.x, ConstantValue.y, interpolationPoint);
                }
            }
        }

        public static implicit operator float(RangeReference reference)
        {
            return reference.UseConstant ? Random.Range(reference.ConstantValue.x, reference.ConstantValue.y) :  (reference.Variable != null ? Random.Range(reference.Variable.Value.x, reference.Variable.Value.y) : Random.Range(reference.ConstantValue.x, reference.ConstantValue.y));
        }
    }
}

