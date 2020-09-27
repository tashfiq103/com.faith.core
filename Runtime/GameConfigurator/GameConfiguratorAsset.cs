namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "GameConfiguratorAsset", menuName = "FAITH/GameConfiguratorAsset")]
    public class GameConfiguratorAsset : ScriptableObject
    {
        #region Public Variables

        public CoreEnums.GameMode gameMode;
        public CoreEnums.LogType logType;


        public CoreEnums.DataSavingMode dataSavingMode;
        public bool notifyInConsoleOnSavingDataSnapshot = true;
        public bool dataSaveWhenApplicationLoseFocus = true;
        public bool dataSaveWhenApplicationQuit = true;
        [Range(1,60)]
        public float snapshotFrequenceyInSec = 15;

        #endregion

    }
}

