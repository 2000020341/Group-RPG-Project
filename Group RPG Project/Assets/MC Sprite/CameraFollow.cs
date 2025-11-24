using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;     // player
    public Vector3 offset;       // initial camera offset from player
    public float smoothTime = 0.15f;
    public float rotateSpeed = 6f;

    private Vector3 velocity = Vector3.zero;
    private float rotationY = 0f;   // horizontal rotation
    private float rotationX = 20f;  // vertical rotation

    public float minVerticalAngle = -10f;  // clamp that hoe
    public float maxVerticalAngle = 45f;

    void LateUpdate()
    {
        if (target == null) return;

        HandleRotation();

        // Apply rotation to offset
        Quaternion rot = Quaternion.Euler(rotationX, rotationY, 0f);
        Vector3 rotatedOffset = rot * offset;

        // Smooth follow
        Vector3 targetPos = target.position + rotatedOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // Maintain
        transform.LookAt(target);
    }

    private void HandleRotation()
    {
        // Only rotate camera while holding right mouse button
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Horizontal
            rotationY += mouseX * rotateSpeed;

            // Vertical
            rotationX -= mouseY * rotateSpeed;

            // Clamp vertical angle to prevent camera flipping over
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        }
    }
}