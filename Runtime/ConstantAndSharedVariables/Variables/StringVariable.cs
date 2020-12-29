namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "String Variable",
        menuName = ScriptableObjectAssetMenu.MENU_SHARED_VARIABLE_STRING,
        order = ScriptableObjectAssetMenu.ORDER_SHARED_VARIABLE_STRING)]
    public class StringVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public string Value;

        public void SetValue(string value)
        {
            Value = value;
        }

        public void SetValue(StringVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(string amount)
        {
            Value += amount;
        }

        public void ApplyChange(StringVariable amount)
        {
            Value += amount.Value;
        }
    }
}


