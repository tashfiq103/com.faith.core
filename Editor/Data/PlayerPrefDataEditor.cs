namespace com.faith.core
{
    using UnityEditor;

    public class PlayerPrefDataEditor : EditorWindow
    {
        [MenuItem("FAITH/PlayerPrefData/Reset", false)]
        public static void ResetPlayerPrefData() {

            PlayerPrefDataSettings.ResetAllPlayerPrefData();
        }

    }
}

