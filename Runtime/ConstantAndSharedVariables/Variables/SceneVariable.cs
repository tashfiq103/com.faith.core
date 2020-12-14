namespace com.faith.core
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    [CreateAssetMenu(
        fileName = "SceneVariable",
        menuName = ScriptableObjectAssetMenu.MENU_SCENE_MANAGEMENT_SCENE_VARIABLE,
        order = ScriptableObjectAssetMenu.ORDER_SCENE_MANAGEMENT_SCENE_VARIABLE)]
    public class SceneVariable : ScriptableObject
    {
        #region Public Variables

        public string SceneName { get { return sceneName; } }

        #endregion

        #region Private Variables

#if UNITY_EDITOR
        [SerializeField] private bool isEnabled;
        [SerializeField] private bool advanceOption;
        [SerializeField] private string scenePath;

#endif

        [SerializeField] private string sceneName;
        [SerializeField] private FloatReference animationSpeedForLoadingBar;
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        #endregion

        #region Public Callback

        public void LoadScene(
            UnityAction<float> OnUpdatingProgression = null,
            UnityAction OnSceneLoaded = null,
            float initalDelayToInvokeOnSceneLoaded = 0)
        {

#if UNITY_EDITOR

            if (!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, loadSceneMode == LoadSceneMode.Single ? UnityEditor.SceneManagement.OpenSceneMode.Single : UnityEditor.SceneManagement.OpenSceneMode.Additive);
            }
            else
            {
                SceneTransitionController.LoadScene(
                    SceneName,
                    OnUpdatingProgression,
                    OnSceneLoaded,
                    animationSpeedForLoadingBar,
                    initalDelayToInvokeOnSceneLoaded,
                    loadSceneMode
                );
            }

#else
            SceneTransitionController.LoadScene(
                    SceneName,
                    OnUpdatingProgression,
                    OnSceneLoaded,
                    animationSpeedForLoadingBar,
                    initalDelayToInvokeOnSceneLoaded,
                    loadSceneMode
                );
#endif

        }

        #endregion

    }
}

