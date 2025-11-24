using UnityEngine;

public class ThirdPersonView : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            // Make sprite face directly toward the camera
            transform.forward = Camera.main.transform.forward;
        }
    }
}
