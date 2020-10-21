namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName    = "Float Variable",
        menuName    = ScriptableObjectAssetMenu.MENU_SHARED_VARIABLE_FLOAT,
        order       = ScriptableObjectAssetMenu.ORDER_SHARED_VARIABLE_FLOAT)]
    public class FloatVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public float Value;

        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
}

