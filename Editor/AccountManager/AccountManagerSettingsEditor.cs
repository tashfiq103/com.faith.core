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

            if (_listOfCurrencyToBeAdded.Count > 0) {

                DrawHorizontalLine();
                EditorGUILayout.LabelField("Currency to be Added :", EditorStyles.boldLabel);
                EditorGUI.indentLevel += 1;
                foreach (AccountManagerSettings.CurrecnyInfo currencyInfo in _listOfCurrencyToBeAdded) {

                    EditorGUILayout.LabelField(currencyInfo.currencyName);
                }
                EditorGUI.indentLevel -= 1;
            }

            DrawHorizontalLine();
            EditorGUILayout.PropertyField(_listOfCurrencyInfos);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Configuretion

        private void AddNewCurrency() {

            int counter = _reference.listOfCurrencyInfos.Count + _listOfCurrencyToBeAdded.Count;
            AccountManagerSettings.CurrecnyInfo newCurrency = new AccountManagerSettings.CurrecnyInfo() {
                currencyName = "CURRENCY_" + counter
            };

            _listOfCurrencyToBeAdded.Add(newCurrency);
            _flagedForGeneratingEnum = true;
        }

        private async void GenerateEnum() {

            int numberOfCurrencyAlreadyInList   = _reference.listOfCurrencyInfos.Count;
            int numberOfCurrencyToBeAdded       = _listOfCurrencyToBeAdded.Count;

           
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

                for (int i = 0; i < numberOfCurrencyAlreadyInList; i++) {

                    scriptData += "\n\t\t" + _reference.listOfCurrencyInfos[i].currencyName.ToUpper();

                    if (i == (numberOfCurrencyAlreadyInList - 1))
                    {
                        if(numberOfCurrencyToBeAdded > 0)
                            scriptData += ",";
                    }
                    else {

                        scriptData += ",";
                    }
                }

                for (int i = 0; i < numberOfCurrencyToBeAdded; i++) {

                    scriptData += "\n\t\t" + _listOfCurrencyToBeAdded[i].currencyName.ToUpper();
                    if (i == 0 && numberOfCurrencyAlreadyInList > 0) {

                        scriptData += ",";
                    } else if (i < (numberOfCurrencyAlreadyInList - 1)) {

                        scriptData += ",";
                    }
                }


                //Ending
                scriptData += "\n\t}";
                scriptData += "\n}";

                await streamWriter.WriteAsync(scriptData);
            }

            AssetDatabase.Refresh();
            await Task.Delay(500);
            while (EditorApplication.isCompiling)
                await Task.Delay(100);

            for (int i = 0; i < numberOfCurrencyToBeAdded; i++) {

                AccountManagerSettings.CurrecnyInfo currencyInfo = _listOfCurrencyToBeAdded[0];
                _reference.listOfCurrencyInfos.Add(currencyInfo);
                _listOfCurrencyToBeAdded.RemoveAt(0);
                _listOfCurrencyInfos.serializedObject.ApplyModifiedProperties();
            }

           
        }

        #endregion

    }
}

