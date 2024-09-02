using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MLib
{
    public class MSceneManager : MSingleton<MSceneManager>
    {
        [SerializeField] [Range(0.1f, 0.9f)] private float percentAccept = 0.85f;

        public Action OnLoadStart;
        public Action OnLoadDone;
        public Action<float> OnProgressChanged;
        public void LoadScene(string sceneName, bool destroyCurrentScene = true)
        {
            StartCoroutine(CR_LoadScene(sceneName, destroyCurrentScene));
        }

        private IEnumerator CR_LoadScene(string sceneName, bool destroyCurrentScene)
        {
            string oldScene = SceneManager.GetActiveScene().name;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            asyncLoad.allowSceneActivation = false;

            OnLoadStart?.Invoke();
            while (!asyncLoad.isDone)
            {
                OnProgressChanged?.Invoke(asyncLoad.progress);
                if (asyncLoad.progress >= percentAccept)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
            OnLoadDone?.Invoke();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            if (destroyCurrentScene)
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);
                asyncUnload.completed += (x) =>
                {
                    OnLoadStart = null;
                    OnLoadDone = null;
                    OnProgressChanged = null;
                };
            }
        }
    }
}
