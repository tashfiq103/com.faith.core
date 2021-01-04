namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "Layer Variable",
        menuName = ScriptableObjectAssetMenu.MENU_UNITY_LAYER_VARIABLE,
        order = ScriptableObjectAssetMenu.ORDER_UNITY_LAYER_VARIABLE)]
    public class LayerVariable : ScriptableObject
    {
        #region Public Variables

#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public string NameOfLayer {

            get
            {
                return _layerName;
            }
        }

        public int Value {
            get
            {
                return LayerMask.NameToLayer(_layerName);
            }
        }

        #endregion



        #region Private Variables

#if UNITY_EDITOR
        //Require To Store The Index In Editor
        [SerializeField] private int _layerIndex;
#endif

        [SerializeField] private string _layerName;

        #endregion

        #region Public Callback

        public void SetValue(string value)
        {
            _layerName = value;
        }

        #endregion
    }
}

