namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "Range Variable",
        menuName = ScriptableObjectAssetMenu.MENU_SHARED_VARIABLE_RANGE,
        order = ScriptableObjectAssetMenu.ORDER_SHARED_VARIABLE_RANGE)]
    public class RangeVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public Vector2 Value;

        [SerializeField] private float min = 0;
        [SerializeField] private float max = 1;

        public float Min { get { return min; } }
        public float Max { get { return max; } }

        public void SetValue(Vector2 value)
        {
            Value = value;
        }

        public void SetValue(RangeVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(Vector2 amount)
        {
            Value += amount;
        }

        public void ApplyChange(RangeVariable amount)
        {
            Value += amount.Value;
        }
    }
}


