namespace com.faith.core
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using System.Threading.Tasks;

    public class SceneTransitionController
    {
        #region Private Variables

        private const float LOAD_READY_PERCENTAGE = 0.9f;

        private static bool _isSceneLoadOperationRunning = false;

        #endregion

        #region Configuretion

        private static async void ControllerForLoadingScene(
            string sceneName,
            UnityAction<float> OnUpdatingProgression,
            UnityAction OnSceneLoaded,
            float animationSpeedForLoadingBar = 1,
            float initalDelayToInvokeOnSceneLoaded = 0,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            animationSpeedForLoadingBar = Mathf.Clamp01(animationSpeedForLoadingBar);
            float animatedLerpValue = 0f;

            AsyncOperation asyncOperationForLoadingScene = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            asyncOperationForLoadingScene.allowSceneActivation = false;
            while (animatedLerpValue <= 0.99f)
            {
                animatedLerpValue = Mathf.Lerp(
                    animatedLerpValue,
                    Mathf.Clamp(asyncOperationForLoadingScene.progress, 0, 0.9f) / LOAD_READY_PERCENTAGE,
                    animationSpeedForLoadingBar);

                OnUpdatingProgression?.Invoke(animatedLerpValue);

                await Task.Delay(1);
            }

            OnUpdatingProgression?.Invoke(1);

            while (asyncOperationForLoadingScene.progress < 0.9f) {

                await Task.Delay(33);
            }

            asyncOperationForLoadingScene.allowSceneActivation = true;

            await Task.Delay(33);

            await Task.Delay((int)(initalDelayToInvokeOnSceneLoaded * 1000));

            OnSceneLoaded?.Invoke();

            _isSceneLoadOperationRunning = false;

        }

        #endregion

        #region Public Callback

        public static void LoadScene(
            string sceneName,
            UnityAction<float> OnUpdatingProgression = null,
            UnityAction OnSceneLoaded = null,
            float animationSpeedForLoadingBar = 1,
            float initalDelayToInvokeOnSceneLoaded = 0,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (!_isSceneLoadOperationRunning)
            {
                _isSceneLoadOperationRunning = true;
                ControllerForLoadingScene(sceneName, OnUpdatingProgression, OnSceneLoaded, animationSpeedForLoadingBar, initalDelayToInvokeOnSceneLoaded, loadSceneMode);
            }
            else {

                CoreDebugger.Debug.LogError("Scene transition already running. Failed to take the request on new loaded scene : " + sceneName);
            }
        }

        #endregion
    }
}

