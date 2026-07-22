using UnityEngine;
using UnityEngine.SceneManagement;
public static class BootstrapLoader
{
    private const string BootstrapSceneName = "Bootstrap";
    
    //private const string MainSceneName = "MainMenu";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnsureBootstrapSceneLoaded(SceneManager.GetActiveScene());
    }
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureBootstrapSceneLoaded(scene);
    }

    private static void EnsureBootstrapSceneLoaded(Scene scene)
    {
        if (!SceneManager.GetSceneByName(BootstrapSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(BootstrapSceneName, LoadSceneMode.Additive);
        }
    }
}
