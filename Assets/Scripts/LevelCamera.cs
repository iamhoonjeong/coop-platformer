using Unity.Cinemachine;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    CinemachineCamera cinemachine;

    void Awake()
    {
        cinemachine = GetComponentInChildren<CinemachineCamera>(true);
        EnableCamera(false);
    }

    public void EnableCamera(bool enable)
    {
        cinemachine.gameObject.SetActive(enable);
    }

    public void SetNewTarget(Transform newTarget)
    {
        cinemachine.Follow = newTarget;
    }
}
