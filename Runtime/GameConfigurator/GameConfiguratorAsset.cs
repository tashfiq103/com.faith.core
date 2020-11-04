namespace com.faith.core
{
    using UnityEngine;

    [CreateAssetMenu(
        fileName = "GameConfiguretionSettings",
        menuName = ScriptableObjectAssetMenu.MENU_GAME_CONFIGURETION_SETTINGS,
        order = ScriptableObjectAssetMenu.ORDER_GAME_CONFIGURETION_SETTINGS)]
    public class GameConfiguratorAsset : ScriptableObject
    {

        #region SerializedField

        [SerializeField] private bool _isUsedByCentralGameConfiguretion = false;
        [SerializeField] private bool _linkWithCentralGameConfiguretion = false;
        [SerializeField] private CoreEnums.GameMode _gameMode = CoreEnums.GameMode.DEBUG;
        [SerializeField] private CoreEnums.LogType _logType = CoreEnums.LogType.Verbose;
        [SerializeField] private CoreEnums.DataSavingMode _dataSavingMode = CoreEnums.DataSavingMode.PlayerPrefsData;

        #endregion

        #region Public Variables 

        public CoreEnums.GameMode gameMode { get { return _linkWithCentralGameConfiguretion ? GameConfiguratorManager.gameMode : _gameMode; } }

        public CoreEnums.LogType logType { get { return _linkWithCentralGameConfiguretion ? GameConfiguratorManager.logType : _logType; } }
        public Color colorForLog = new Color();
        public Color colorForWarning = Color.yellow;
        public Color colorForLogError = Color.red;

        public CoreEnums.DataSavingMode dataSavingMode { get { return _linkWithCentralGameConfiguretion ? GameConfiguratorManager.dataSavingMode : _dataSavingMode; } }
        public bool dataSaveWhenSceneUnloaded = true;
        public bool dataSaveWhenApplicationLoseFocus = true;
        public bool dataSaveWhenApplicationQuit = true;
        [Range(1,60)]
        public float snapshotFrequenceyInSec = 15;

        #endregion

    }
}

