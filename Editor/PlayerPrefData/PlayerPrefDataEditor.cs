namespace com.faith.core
{
    using UnityEditor;

    public class PlayerPrefDataEditor : BaseEditorWindowClass
    {

        public override void OnEnable() {

            base.OnEnable();
        }

        [MenuItem("FAITH/Core/PlayerPrefs/Clear", priority = 1, validate = false)]
        public static void ClearPlayerPref() {

            bool result = EditorUtility.DisplayDialog(
                "Clear 'PlayerPrefs' (Partial)",
                "Clear 'PlayerPrefs' that are only created by 'PlayerPrefsData/SavedData' using com.faith.core package.",
                "Clear", "Cancel");

            if (result) {
                PlayerPrefDataSettings.ResetAllPlayerPrefData();
            }
        }

        [MenuItem("FAITH/Core/PlayerPrefs/Clear All PlayerPrefs", priority = 2, validate = false)]
        public static void ClearAllPlayerPrefs()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
        }

    }
}

