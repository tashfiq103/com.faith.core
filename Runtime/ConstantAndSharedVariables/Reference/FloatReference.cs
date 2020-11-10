namespace com.faith.core
{

    [System.Serializable]
    public class FloatReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariable Variable;

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public float Value
        {
            get {
                if (UseConstant)
                    return ConstantValue;
                else {
                    if (Variable != null)
                        return Variable.Value;
                    else {
                        CoreDebugger.Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                        return ConstantValue;
                    }
                }
            }
        }

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}

