namespace com.faith.core
{

    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

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
            get
            {
                if (UseConstant)
                    return sceneName;
                else
                {
                    if (Variable != null)
                        return Variable.sceneName;
                    else
                    {
                        CoreDebugger.Debug.LogWarning("Variable (ScriptableObject) not assigned, returning 'ConstantValue'.");
                        return sceneName;
                    }
                }
            }
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

            if (UseConstant)
            {
#if UNITY_EDITOR

                if (!UnityEditor.EditorApplication.isPlaying)
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
                        initalDelayToInvokeOnSceneLoaded,
                        LoadSceneMode.Single
                    );
                }

#else
            SceneTransitionController.LoadScene(
                        sceneName,
                        OnUpdatingProgression,
                        OnSceneLoaded,
                        animationSpeedForLoadingBar,
                        initalDelayToInvokeOnSceneLoaded,
                        LoadSceneMode.Single
                    );
#endif
            }
            else {

                Variable.LoadScene(OnUpdatingProgression, OnSceneLoaded, initalDelayToInvokeOnSceneLoaded);
            }
        }

        public static implicit operator string(SceneReference reference)
        {

            return reference.SceneName;
        }

        #endregion

        
    }
}

