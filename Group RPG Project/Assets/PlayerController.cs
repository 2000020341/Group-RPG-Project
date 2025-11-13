using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDistance;

    public LayerMask terrainLayer;
    private Rigidbody rb;
    private SpriteRenderer sr;

    private Vector2 moveInput;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // NEW INPUT SYSTEM METHOD - Called automatically by PlayerInput component
    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 castPosition = transform.position;
        castPosition.y += 0.1f;

        if (Physics.Raycast(castPosition, -transform.up, out hit, Mathf.Infinity, terrainLayer))
    {
        if (hit.collider != null)
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + groundDistance;
            transform.position = pos;
        }
    }

        // Movement
        float x = moveInput.x;
        float y = moveInput.y;
        Vector3 moveDir = new Vector3(x, 0, y);

        transform.position += moveDir * speed * Time.deltaTime;

        // Sprite flipping
        if (x < 0) sr.flipX = true;
        else if (x > 0) sr.flipX = false;

    }
}
