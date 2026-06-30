using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string TargetScene;
    [SerializeField] private string SpawnID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RoomTransitionManager manager = collision.GetComponent<RoomTransitionManager>();
        if (manager != null)
        {
            manager.EnterRoom(TargetScene, SpawnID);
        }
    }
}
