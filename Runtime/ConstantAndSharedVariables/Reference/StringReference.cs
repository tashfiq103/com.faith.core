namespace com.faith.core
{
    [System.Serializable]
    public class StringReference
    {
        #region Public Variables

        public string ConstantValue;

        public bool UseConstant = true;
        public StringVariable Variable;

        #endregion

        #region Public Callback

        public StringReference(string value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        [UnityEngine.SerializeField]
        public string Value
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
                        CoreDebugger.Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                        return ConstantValue;
                    }
                }
            }
        }

        public static implicit operator string(StringReference reference)
        {
            return reference.Value;
        }

        #endregion
    }
}

