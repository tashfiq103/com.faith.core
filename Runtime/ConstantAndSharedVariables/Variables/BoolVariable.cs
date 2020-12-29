namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "Bool Variable",
        menuName = ScriptableObjectAssetMenu.MENU_SHARED_VARIABLE_BOOLEAN,
        order = ScriptableObjectAssetMenu.ORDER_SHARED_VARIABLE_BOOLEAN)]
    public class BoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public bool Value;

        public void SetValue(bool value)
        {
            Value = value;
        }

        public void SetValue(BoolVariable value)
        {
            Value = value.Value;
        }
    }
}

