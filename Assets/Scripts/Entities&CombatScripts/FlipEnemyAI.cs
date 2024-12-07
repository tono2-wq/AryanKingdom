using UnityEngine;

public class FlipEnemyAI : MonoBehaviour
{
    public float movementThreshold = 0.1f;  // The threshold to determine if movement is significant
    private Vector2 previousPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousPosition = transform.position;  // Initialize the previous position
    }

    void Update()
    {
        FlipBasedOnMovement();
    }

    void FlipBasedOnMovement()
    {
        // Get current movement direction
        Vector2 movementDirection = (Vector2)transform.position - previousPosition;

        // Flip the sprite based on the movement in the horizontal direction
        if (movementDirection.x > movementThreshold)
        {
            // Moving right, face right
            spriteRenderer.flipX = false;
        }
        else if (movementDirection.x < -movementThreshold)
        {
            // Moving left, face left
            spriteRenderer.flipX = true;
        }

        // Update previous position for the next frame
        previousPosition = transform.position;
    }
}