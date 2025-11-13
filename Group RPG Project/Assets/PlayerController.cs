using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDistance;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

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
        castPosition.y += 1;
        if (Physics.Raycast(castPosition, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePosition = transform.position;
                movePosition.y = hit.point.y + groundDistance;
                transform.position = movePosition;
            }
        }

        // NEW INPUT SYSTEM - Use stored input values
        float x = moveInput.x;
        float y = moveInput.y;
        Vector3 moveDir = new Vector3(x, 0, y);
        rb.linearVelocity = moveDir * speed;

        if (x != 0 && x < 0) //flip sprite depending on direction you move
        {
            sr.flipX = true;
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false;
        }
    }
}
