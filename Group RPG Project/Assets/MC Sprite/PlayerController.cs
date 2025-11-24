using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("The movement speed of the character.")]
    public float speed = 5.0f;

    [Header("Directional Sprites")]
    public Sprite backSprite;
    public Sprite frontSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Walking Sprites")]
    public Sprite[] walkUpSprites;
    public Sprite[] walkDownSprites;
    public Sprite[] walkRightSprites;
    public Sprite[] walkLeftSprites;

    public float walkFrameRate = 8f;

    [Header("Audio")]
    public AudioClip walkingSound;

    private const float deadZone = 0.1f;

    private Rigidbody rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private Vector3 moveInput;
    private Vector3 lastMoveDirection = Vector3.forward;

    private float walkTimer = 0f;
    private int walkFrameIndex = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (sr == null)
        {
            Debug.LogError("PlayerController could NOT find a SpriteRenderer in any child objects.");
        }

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        moveInput = new Vector3(horizontalInput, 0, verticalInput);

        if (moveInput.magnitude > deadZone)
        {
            lastMoveDirection = moveInput;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * speed * Time.fixedDeltaTime);
    }

    // -----------------------------------------------------------
    // ANIMATION HELPERS
    // -----------------------------------------------------------

    private void PlayForcedWalkUpAnimation()
    {
        if (walkUpSprites == null || walkUpSprites.Length == 0)
            return;

        walkTimer += Time.deltaTime;

        if (walkTimer >= 1f / walkFrameRate)
        {
            walkTimer = 0f;
            walkFrameIndex++;

            if (walkFrameIndex >= walkUpSprites.Length)
                walkFrameIndex = 0;

            sr.sprite = walkUpSprites[walkFrameIndex];
        }
    }

    private void UpdateWalkingAnimation()
    {
        Sprite[] currentWalkSet = null;

        // Pick animation based on movement direction
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.z))
        {
            currentWalkSet = moveInput.x > 0 ? walkRightSprites : walkLeftSprites;
        }
        else
        {
            currentWalkSet = moveInput.z > 0 ? walkUpSprites : walkDownSprites;
        }

        if (currentWalkSet == null || currentWalkSet.Length == 0)
            return;

        walkTimer += Time.deltaTime;

        if (walkTimer >= 1f / walkFrameRate)
        {
            walkTimer = 0f;
            walkFrameIndex++;

            if (walkFrameIndex >= currentWalkSet.Length)
                walkFrameIndex = 0;

            sr.sprite = currentWalkSet[walkFrameIndex];
        }
    }

    // -----------------------------------------------------------
    // MAIN ANIMATION LOGIC
    // -----------------------------------------------------------

    void LateUpdate()
    {
        bool isMoving = moveInput.magnitude > deadZone;

        if (isMoving)
        {
            // AUDIO
            if (walkingSound != null && !audioSource.isPlaying)
            {
                audioSource.clip = walkingSound;
                audioSource.Play();
            }

            // ANIMATIONS
            if (CameraFollow.isRotating)
            {
                // Force walk-up animation while rotating camera
                PlayForcedWalkUpAnimation();
            }
            else
            {
                // Standard directional walking animation
                UpdateWalkingAnimation();
            }
        }
        else
        {
            // Stop walking audio
            if (audioSource.isPlaying)
                audioSource.Stop();

            // IDLE ANIMATION
            if (CameraFollow.isRotating)
            {
                // Idle always faces UP if camera is rotating
                sr.sprite = backSprite;
            }
            else
            {
                // Normal idle face direction
                if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.z))
                {
                    sr.sprite = lastMoveDirection.x > 0 ? rightSprite : leftSprite;
                }
                else
                {
                    sr.sprite = lastMoveDirection.z > 0 ? backSprite : frontSprite;
                }
            }

            // Reset walking animation frame
            walkFrameIndex = 0;
            walkTimer = 0f;
        }
    }
}
