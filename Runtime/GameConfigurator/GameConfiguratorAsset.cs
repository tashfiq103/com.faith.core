namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "GameConfiguretionSettings",
        menuName = ScriptableObjectAssetMenu.MENU_GAME_CONFIGURETION_SETTINGS,
        order = ScriptableObjectAssetMenu.ORDER_GAME_CONFIGURETION_SETTINGS)]
    public class GameConfiguratorAsset : ScriptableObject
    {
        #region Public Variables

        public CoreEnums.GameMode gameMode;
        public CoreEnums.LogType logType;
        public CoreEnums.DataSavingMode dataSavingMode;

        public bool dataSaveWhenSceneUnloaded = true;
        public bool dataSaveWhenApplicationLoseFocus = true;
        public bool dataSaveWhenApplicationQuit = true;
        [Range(1,60)]
        public float snapshotFrequenceyInSec = 15;

        #endregion

    }
}

