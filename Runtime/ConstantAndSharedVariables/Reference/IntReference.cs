namespace com.faith.core
{
    using UnityEngine;

    [System.Serializable]
    public class IntReference
    {
        public bool UseConstant = true;
        public int ConstantValue;
        public IntVariables Variable;

        public IntReference()
        { }

        public IntReference(int value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public int Value
        {
            get
            {
                if (UseConstant)
                    return ConstantValue;
                else
                {
                    if (Variable != null)
                        return Variable.Value;
                    else
                    {
                        Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                        return ConstantValue;
                    }
                }
            }
        }

        public static implicit operator int(IntReference reference)
        {
            return reference.Value;
        }
    }
}

