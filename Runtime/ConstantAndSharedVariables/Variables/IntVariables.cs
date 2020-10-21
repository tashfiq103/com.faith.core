namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "Int Variable",
        menuName = ScriptableObjectAssetMenu.MENU_SHARED_VARIABLE_INT,
        order = ScriptableObjectAssetMenu.ORDER_SHARED_VARIABLE_INT)]
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

