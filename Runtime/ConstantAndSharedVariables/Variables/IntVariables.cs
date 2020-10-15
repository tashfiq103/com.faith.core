namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "IntVariables", menuName = "FAITH/SharedVariable/IntVariables")]
    public class IntVariables : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public int Value;

        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetValue(IntVariables value)
        {
            Value = value.Value;
        }

        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        public void ApplyChange(IntVariables amount)
        {
            Value += amount.Value;
        }
    }
}

