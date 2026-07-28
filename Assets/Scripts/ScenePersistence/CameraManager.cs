using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vCam;
    private CinemachineConfiner2D confiner;

    void Awake()
    {
        confiner = vCam.GetComponent<CinemachineConfiner2D>();
    }


    public void SetConfiner(Collider2D newConfiner)
    {
        confiner.BoundingShape2D = newConfiner;

    }  
}
