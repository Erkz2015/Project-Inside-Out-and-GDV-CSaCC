using UnityEngine;

public class LockColliderRotation : MonoBehaviour
{
    void Update()
    {
        Vector3 rot = transform.eulerAngles;

        rot.x = 0f;
        rot.z = 0f;

        transform.eulerAngles = rot;
    }
}