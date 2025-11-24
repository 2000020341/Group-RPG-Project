using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     // MC
    public Vector3 offset;       // distance between camera and player
    public float smoothTime = 0.2f;

    public static bool isRotating = false;
    public float rotateSpeed = 6f;

    private float currentRotation = 0f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation();

        Vector3 rotatedOffset = Quaternion.Euler(0f, currentRotation, 0f) * offset;

        // Smooth follow
        Vector3 targetPos = target.position + rotatedOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // Always look at the player
        transform.LookAt(target);
    }

    private void HandleRotation()
    {
        // Mouse always rotates camera
        float rotationInput = Input.GetAxis("Mouse X") * rotateSpeed;

        currentRotation += rotationInput;

        // Detect real rotation motion
        isRotating = Mathf.Abs(rotationInput) > 0.001f;
    }
}
