namespace com.faith.core
{
    using UnityEngine;
    using UnityEngine.Events;

    [CreateAssetMenu(fileName = "SceneVariable", menuName = "FAITH/SharedVariable/SceneVariable")]
    public class SceneVariable : ScriptableObject
    {
        #region Public Variables

#if UNITY_EDITOR
        public string scenePath;
#endif
        public string sceneName;

        #endregion

        #region Public Callback

        public void LoadScene(
            UnityAction<float> OnUpdatingProgression = null,
            UnityAction OnSceneLoaded = null,
            float animationSpeedForLoadingBar = 1,
            float initalDelayToInvokeOnSceneLoaded = 0) {

#if UNITY_EDITOR

            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
            }
            else {
                SceneTransitionController.LoadScene(
                    sceneName,
                    OnUpdatingProgression,
                    OnSceneLoaded,
                    animationSpeedForLoadingBar,
                    initalDelayToInvokeOnSceneLoaded
                );
            }

#else
            SceneTransitionController.LoadScene(
                    sceneName,
                    OnUpdatingProgression,
                    OnSceneLoaded,
                    animationSpeedForLoadingBar,
                    initalDelayToInvokeOnSceneLoaded
                );
#endif

        }

        #endregion

    }
}

