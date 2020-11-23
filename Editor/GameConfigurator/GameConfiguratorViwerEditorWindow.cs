namespace com.faith.core
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    public class GameConfiguratorViwerEditorWindow : BaseEditorWindowClass
    {
        #region Private Variables

        private static GameConfiguratorViwerEditorWindow EditorWindow;

        #endregion

        #region Configuretion

        private static bool IsAnyGameConfiguretionAssetUsedByGameConfiguretionManager() {

            List<GameConfiguratorAsset> gameConfiguratorAssets = GetAsset<GameConfiguratorAsset>();
            foreach (GameConfiguratorAsset gameConfigAsset in gameConfiguratorAssets) {

                if (gameConfigAsset.EditorAccessIfUsedByCentralGameConfiguretion)
                    return true;
            }
            CoreDebugger.Debug.LogError("Please assign any of your 'GameConfiguretionAsset' to 'GameConfiguretionManager'", prefix : "GameConfiguretorAsset");
            return false;
        }

        private static void SetLinkStatusWithCentralGameConfiguretion(bool statusFlag) {

            if (IsAnyGameConfiguretionAssetUsedByGameConfiguretionManager()) {

                List<GameConfiguratorAsset> gameConfiguratorAssets = GetAsset<GameConfiguratorAsset>();

                foreach (GameConfiguratorAsset gameConfigAsset in gameConfiguratorAssets)
                {
                    if (!gameConfigAsset.EditorAccessIfUsedByCentralGameConfiguretion)
                    {
                        SerializedObject serializedGameConfiguretorAsset = new SerializedObject(gameConfigAsset);

                        SerializedProperty _linkWithCentralGameConfiguretion = serializedGameConfiguretorAsset.FindProperty("_linkWithCentralGameConfiguretion");
                        _linkWithCentralGameConfiguretion.boolValue = statusFlag;
                        _linkWithCentralGameConfiguretion.serializedObject.ApplyModifiedProperties();

                        serializedGameConfiguretorAsset.ApplyModifiedProperties();
                    }
                }
            }
        }

        #endregion

        #region EditorWindow

        [MenuItem("FAITH/Core/GameConfigurator/Viwer", priority = 0)]
        public static void ShowWindow()
        {

            EditorWindow = GetWindow<GameConfiguratorViwerEditorWindow>("GameConfiguretor Viwer", typeof(GameConfiguratorViwerEditorWindow));

            EditorWindow.minSize = new Vector2(450f, 240f);
            EditorWindow.Show();
        }

        [MenuItem("FAITH/Core/GameConfigurator/Link : Production", priority = 1)]
        public static void LinkWithProductionGameConfiguretor() {

            SetLinkStatusWithCentralGameConfiguretion(true);
        }

        [MenuItem("FAITH/Core/GameConfigurator/Unlink : Production", priority = 1)]
        public static void UnlinkWithProductionGameConfiguretor()
        {
            SetLinkStatusWithCentralGameConfiguretion(false);
        }

        #endregion
    }
}

