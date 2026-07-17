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
            Debug.Log($"Entering room: {TargetScene} with spawn ID: {SpawnID}");
            manager.EnterRoom(TargetScene, SpawnID);
        }
        else
        {
            Debug.LogWarning("RoomTransitionManager not found on the colliding object.");
        }
    }
}
