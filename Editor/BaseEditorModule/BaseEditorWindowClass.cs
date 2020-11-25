namespace com.faith.core
{
    using UnityEditor;

    public class BaseEditorWindowClass : EditorWindow
    {

        protected static CoreEnums.CorePackageStatus _packageStatus = CoreEnums.CorePackageStatus.InDevelopment;

        #region OnEditor

        public virtual void OnEnable()
        {
            if (_packageStatus == CoreEnums.CorePackageStatus.InDevelopment)
                _packageStatus = AssetDatabase.FindAssets("com.faith.core", new string[] { "Packages" }).Length > 0 ? CoreEnums.CorePackageStatus.Production : CoreEnums.CorePackageStatus.InDevelopment;
        }

        #endregion

    }
}