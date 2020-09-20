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

        #endregion

    }
}

