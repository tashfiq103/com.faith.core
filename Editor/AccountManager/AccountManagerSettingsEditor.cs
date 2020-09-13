namespace com.faith.core
{
    using System.IO;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEditor;


    [CustomEditor(typeof(AccountManagerSettings))]
    public class AccountManagerSettingsEditor : BaseEditorClass
    {

        #region Private Variables
        private AccountManagerSettings  _reference;

        private SerializedProperty      _dataSavingMode;
        private SerializedProperty      _listOfCurrencyInfos;

        private static bool _flagedForGeneratingEnum = false;
        private static List<AccountManagerSettings.CurrecnyInfo> _listOfCurrencyToBeAdded;

        #endregion

        #region OnEditor

        private void OnEnable()
        {
            _reference = (AccountManagerSettings)target;

            if (_reference == null)
                return;

            if (_listOfCurrencyToBeAdded == null)
                _listOfCurrencyToBeAdded = new List<AccountManagerSettings.CurrecnyInfo>();

            _dataSavingMode = serializedObject.FindProperty("dataSavingMode");
            _listOfCurrencyInfos = serializedObject.FindProperty("listOfCurrencyInfos");
            if (_reference.listOfCurrencyInfos == null) {

                _reference.listOfCurrencyInfos = new List<AccountManagerSettings.CurrecnyInfo>();
                _listOfCurrencyInfos.serializedObject.ApplyModifiedProperties();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(_dataSavingMode);
                if (GUILayout.Button("+Add", GUILayout.Width(50f))){

                    AddNewCurrency();
                }
                if (_flagedForGeneratingEnum && GUILayout.Button("GenerateEnum", GUILayout.Width(125))) {

                    _flagedForGeneratingEnum = false;
                    GenerateEnum();
                }
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Configuretion

        private void AddNewCurrency() {

            _listOfCurrencyToBeAdded.Add(new AccountManagerSettings.CurrecnyInfo());
            _flagedForGeneratingEnum = true;
        }

        private async void GenerateEnum() {

            string path = Application.dataPath + "/AccountManagerEnums";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += "/AccountManagerCurrencyEnum.cs";

            using (StreamWriter streamWriter = new StreamWriter(path)) {

                string scriptData = "namespace com.faith.core";
                scriptData += "\n{";
                scriptData += "\n\tpublic enum AccountManagerCurrencyEnum";
                scriptData += "\n\t{";
                //Starting

                foreach (AccountManagerSettings.CurrecnyInfo currencyInfo in _reference.listOfCurrencyInfos)
                    scriptData += "\n\t\t" + currencyInfo.currencyName.ToUpper();

                foreach (AccountManagerSettings.CurrecnyInfo currencyInfo in _listOfCurrencyToBeAdded)
                    scriptData += "\n\t\t" + currencyInfo.currencyName.ToUpper();

                //Ending
                scriptData += "\n\t}";
                scriptData += "\n}";

                await streamWriter.WriteAsync(scriptData);
            }

            AssetDatabase.Refresh();
            await Task.Delay(500);
            while (EditorApplication.isCompiling)
                await Task.Delay(100);

            int numberOfCurrencyToBeAdded = _listOfCurrencyToBeAdded.Count;
            for (int i = 0; i < numberOfCurrencyToBeAdded; i++) {

                _reference.listOfCurrencyInfos.Add(_listOfCurrencyToBeAdded[0]);
                _listOfCurrencyToBeAdded.RemoveAt(0);
            }

            _listOfCurrencyInfos.serializedObject.ApplyModifiedProperties();
        }

        #endregion

    }
}

