namespace com.faith.core
{
    using UnityEngine;

    [System.Serializable]
    public class LayerReference
    {
        #region Private Variables

#if UNITY_EDITOR

        [SerializeField] private int _layerIndex;
#endif

        #endregion

        #region Public Variables

        public bool UseConstant = true;

        public string           ConstantValue;
        public LayerVariable    Variable;

        #endregion

        #region Public Callback

        public LayerReference(string value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public string NameOfLayer
        {
            get
            {
                if (UseConstant)
                    return ConstantValue;
                else {

                    if (Variable != null)
                        return Variable.NameOfLayer;
                    else
                    {
                        Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                        return ConstantValue;
                    }
                }
            }
        }

        public int Value
        {
            get
            {
                if (UseConstant)
                    return LayerMask.NameToLayer(ConstantValue);
                else
                {
                    if (Variable != null)
                        return Variable.Value;
                    else
                    {
                        Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                        return LayerMask.NameToLayer(ConstantValue);
                    }
                }
            }
        }

        public static implicit operator int(LayerReference reference)
        {
            return reference.Value;
        }

        #endregion
    }
}

