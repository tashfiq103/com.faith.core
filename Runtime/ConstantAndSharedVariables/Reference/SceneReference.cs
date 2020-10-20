namespace com.faith.core
{

    using UnityEngine.Events;

    [System.Serializable]
    public class SceneReference
    {
        #region Public Variable

#if UNITY_EDITOR
        public string scenePath;
#endif
        public string sceneName;


        //Control Variable
        public bool             UseConstant = true;
        public SceneVariable    Variable;

        public string SceneName
        {
            get { return UseConstant ? sceneName : Variable.sceneName; }
        }

        #endregion


        #region Public Callback

        public SceneReference() { }

        public SceneReference(string value)
        {

            UseConstant = true;
            sceneName = value;
        }

        public void LoadScene(
            UnityAction<float> OnUpdatingProgression = null,
            UnityAction OnSceneLoaded = null,
            float animationSpeedForLoadingBar = 1,
            float initalDelayToInvokeOnSceneLoaded = 0)
        {

#if UNITY_EDITOR

            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
            }
            else
            {
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

        public static implicit operator string(SceneReference reference)
        {

            return reference.SceneName;
        }

        #endregion

        
    }
}

