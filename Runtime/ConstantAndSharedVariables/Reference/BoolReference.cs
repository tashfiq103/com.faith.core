namespace com.faith.core
{
    using UnityEngine;

    [System.Serializable]
    public class BoolReference
    {
        #region Public Variables

        public bool ConstantValue;

        public bool UseConstant = true;
        public BoolVariable Variable;

        #endregion

        #region Public Callback

        public BoolReference(bool value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        [UnityEngine.SerializeField]
        public bool Value
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

        public static implicit operator bool(BoolReference reference)
        {
            return reference.Value;
        }

        #endregion
    }
}


