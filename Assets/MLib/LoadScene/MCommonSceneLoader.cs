#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace MLib
{
    [InitializeOnLoad]
    public class MCommonSceneLoader
    {
        static MCommonSceneLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                var commonScene = SceneManager.GetSceneByName(ConstraintSceneName.Common);
                Debug.Log("On Exit edit mode");
                if (!commonScene.isLoaded)
                {
                    var asyncCommon = SceneManager.LoadSceneAsync(ConstraintSceneName.Common, LoadSceneMode.Additive);

                    asyncCommon.completed += (x) =>
                    {
                        EventsCenter.OnSceneLoaded?.Invoke();
                        Debug.Log("Common scene loaded");
                    };

                }
            }
        }
    }
}
