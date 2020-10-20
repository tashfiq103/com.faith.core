namespace com.faith.core
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    [CreateAssetMenu(fileName = "SceneVariable", menuName = "FAITH/SharedVariable/SceneVariable")]
    public class SceneVariable : ScriptableObject
    {
        #region Public Variables

#if UNITY_EDITOR
        public bool     isEnabled;
        public bool     advanceOption;
        public string   scenePath;
#endif
        public string           sceneName;
        [Range(0.1f,1f)]
        public float animationSpeedForLoadingBar = 0.5f;
        public LoadSceneMode    loadSceneMode= LoadSceneMode.Single;

        #endregion

        #region Public Callback

        public void LoadScene(
            UnityAction<float> OnUpdatingProgression = null,
            UnityAction OnSceneLoaded = null,
            float initalDelayToInvokeOnSceneLoaded = 0) {

#if UNITY_EDITOR

            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, loadSceneMode == LoadSceneMode.Single ? UnityEditor.SceneManagement.OpenSceneMode.Single : UnityEditor.SceneManagement.OpenSceneMode.Additive);
            }
            else {
                SceneTransitionController.LoadScene(
                    sceneName,
                    OnUpdatingProgression,
                    OnSceneLoaded,
                    animationSpeedForLoadingBar,
                    initalDelayToInvokeOnSceneLoaded,
                    loadSceneMode
                );
            }

#else
            SceneTransitionController.LoadScene(
                    sceneName,
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

