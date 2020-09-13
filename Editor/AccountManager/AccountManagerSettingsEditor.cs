namespace com.faith.core
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;


    [CustomEditor(typeof(AccountManagerSettings))]
    public class AccountManagerSettingsEditor : BaseEditorClass
    {

        #region Private Variables
        private AccountManagerSettings  _reference;

        private SerializedProperty      _listOfCurrencyInfos;

        #endregion

        #region OnEditor

        private void OnEnable()
        {
            _reference = (AccountManagerSettings)target;

            if (_reference == null)
                return;

            _listOfCurrencyInfos = serializedObject.FindProperty("listOfCurrencyInfos");
            if (_reference.listOfCurrencyInfos == null) {

                _reference.listOfCurrencyInfos = new List<AccountManagerSettings.CurrecnyInfo>();
                _listOfCurrencyInfos.serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    }
}

