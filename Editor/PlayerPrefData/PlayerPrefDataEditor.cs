namespace com.faith.core
{
    using UnityEditor;

    public class PlayerPrefDataEditor : BaseEditorWindowClass
    {

        public override void OnEnable() {

            base.OnEnable();
        }

        [MenuItem("FAITH/PlayerPrefData/Reset", false)]
        public static void ResetPlayerPrefData() {

            PlayerPrefDataSettings.ResetAllPlayerPrefData();
        }

    }
}

