namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "GameConfiguratorAsset", menuName = "FAITH/GameConfiguratorAsset")]
    public class GameConfiguratorAsset : ScriptableObject
    {
        #region Custom Variables

        public enum GameMode
        {
            DEBUG,
            PRODUCTION
        }

        #endregion

        #region Public Variables

        public GameMode gameMode;

        #endregion

    }
}

