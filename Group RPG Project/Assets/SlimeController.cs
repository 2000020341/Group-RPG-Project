using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class SlimeController : MonoBehaviour
{
    [Header("Animation")]
    [Tooltip("The sequence of sprites for the bounce animation.")]
    public Sprite[] animationFrames;

    [Tooltip("The frame rate of the animation.")]
    public float framesPerSecond = 7.0f;

    [Header("Movement")]
    [Tooltip("The upward force of the slime's jump.")]
    public float jumpForce = 5.0f;

    [Tooltip("The time in seconds between each jump.")]
    public float jumpInterval = 2.0f;

    private Rigidbody rb;
    private SpriteRenderer sr;
    private int currentFrame = 0;
    private float animationTimer = 0f;
    private float jumpTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        // Freeze all rotation and horizontal movement to keep the slime upright and in place.
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    void Start()
    {
        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogError("SlimeController: No animation frames have been assigned.");
            enabled = false; // Disable the script if no frames are set
            return;
        }
    }

    void Update()
    {
        // Animation
        animationTimer += Time.deltaTime;
        if (animationTimer >= 1f / framesPerSecond)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            sr.sprite = animationFrames[currentFrame];
        }

        // Jumping
        jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpInterval)
        {
            jumpTimer = 0f;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
