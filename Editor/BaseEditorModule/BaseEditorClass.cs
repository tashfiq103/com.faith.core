namespace com.faith.core {
    using UnityEditor;

    public class BaseEditorClass : Editor {

        protected static CoreEnums.CorePackageStatus _packageStatus = CoreEnums.CorePackageStatus.InDevelopment;
        protected float singleLineHeight;

        #region OnEditor

        public virtual void OnEnable()
        {
            if (_packageStatus == CoreEnums.CorePackageStatus.InDevelopment)
                _packageStatus = AssetDatabase.FindAssets("com.faith.core", new string[] { "Packages" }).Length > 0 ? CoreEnums.CorePackageStatus.Production : CoreEnums.CorePackageStatus.InDevelopment;

            singleLineHeight = EditorGUIUtility.singleLineHeight;

        }

        #endregion

    }
}