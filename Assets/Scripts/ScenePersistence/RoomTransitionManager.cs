using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private CameraManager cameraManager;
    private string currentRoom = "Room1";
    void Start()
    {
        //EnterRoom("", "");
        SetupCameraConfiner();
    }

    public void EnterRoom(string sceneName, string spawnID)
    {
        StartCoroutine(Transition(sceneName, spawnID));
    }
    public IEnumerator Transition(string sceneName, string spawnID)
    {
        yield return screenFader.Fade(0f, 1f, 0.5f);
        if (!string.IsNullOrEmpty(currentRoom))
        {
            yield return SceneManager.UnloadSceneAsync(currentRoom);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        Scene newScene = SceneManager.GetSceneByName(sceneName);
        if(newScene.IsValid())
        {
            SceneManager.SetActiveScene(newScene);
        }
        currentRoom = SceneManager.GetActiveScene().name;
        SetupRoom(spawnID);
        SetupCameraConfiner();
        if (!string.IsNullOrEmpty(currentRoom))
        {
            yield return new WaitForSeconds(.25f);
        }
        
        yield return screenFader.Fade(1f, 0f, 1f);


        
    }

    private void SetupRoom(string spawnID)
    {
        SpawnPoint [] spawns = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        //SpawnPoint spawnToUse = spawns[0];
        if(spawns.Length == 0)
        {
            Debug.LogWarning("No spawn points found in the scene.");
            return;
        }
        if(!string.IsNullOrEmpty(spawnID))
        {
            foreach(SpawnPoint spawn in spawns)
            {
                if(spawn.spawnID == spawnID)
                {
                    //spawnToUse = spawn;
                    transform.position = spawn.transform.position;
                    break;
                }
            }
            
        }

    }

    private void SetupCameraConfiner()
    {
        CameraConfinerProvider provider = FindFirstObjectByType<CameraConfinerProvider>();
        cameraManager.SetConfiner(provider.confiner);
        
    }
}
