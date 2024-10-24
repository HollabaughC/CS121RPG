using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;  // Disable gravity
    }

    void Update()
    {
        // Get input for movement
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down arrows

        // Normalize the movement vector to ensure consistent speed in diagonal directions
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move the character based on input
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // This is a placeholder for the function that an animation event could call
    public void OnAnimationEventTriggered()
    {
        Debug.Log("Animation event triggered.");
    }
}
