using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MLib
{
    public class MSceneManager : MSingleton<MSceneManager>
    {
        [SerializeField] [Range(0.1f, 0.9f)] private float percentAccept = 0.85f;

        public Action OnLoadStart;
        public Action OnLoadDone;
        public Action OnSceneReady;
        public Action<float> OnProgressChanged;

        private bool enableLoadNewScene = false;

        public void LoadScene(string sceneName, bool destroyCurrentScene = true)
        {
            enableLoadNewScene = true;
            StartCoroutine(CR_LoadScene(sceneName, destroyCurrentScene));
        }

        public async void LoadScene(string sceneName, float minDuration, bool destroyCurrentScene = true)
        {
            StartCoroutine(CR_LoadScene(sceneName, destroyCurrentScene));
            enableLoadNewScene = false;
            await UniTask.WaitForSeconds(minDuration);
            enableLoadNewScene = true;
        }

        private IEnumerator CR_LoadScene(string sceneName, bool destroyCurrentScene)
        {
            if (ScreenFader.Instance) ScreenFader.Instance.FadeIn(0.25f);
            string oldScene = SceneManager.GetActiveScene().name;
            if (destroyCurrentScene)
            {
                var unloadAsync = SceneManager.UnloadSceneAsync(oldScene);
                while (!unloadAsync.isDone) yield return null;
            }
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            asyncLoad.allowSceneActivation = false;


            OnLoadStart?.Invoke();
            while (!asyncLoad.isDone)
            {
                OnProgressChanged?.Invoke(asyncLoad.progress);
                if (asyncLoad.progress >= percentAccept && enableLoadNewScene && !asyncLoad.allowSceneActivation)
                {

                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }

            OnLoadDone?.Invoke();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            if (ScreenFader.Instance) ScreenFader.Instance.FadeOut(0.5f);

            int frameCount = 2;
            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
            OnSceneReady?.Invoke();
            EventsCenter.OnSceneLoaded?.Invoke();

            OnLoadStart = null;
            OnLoadDone = null;
            OnProgressChanged = null;
            OnSceneReady = null;
        }
    }
}
