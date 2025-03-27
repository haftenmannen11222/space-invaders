using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementEnemies : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float dropDistance = 3.8f;
    public float leftBound = -30f;
    public float rightBound = 25f;

    private enum MovementState
    {
        MovingHorizontal,
        Pausing,
        MovingDown
    }

    private MovementState currentState = MovementState.MovingHorizontal;
    private bool movingRight = true;
    private float targetY;
    private float pauseTimer = 0f;
    private float pauseDuration = 0.5f; // 500ms pause

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set gravity scale to 0 to prevent falling
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
    }

    void FixedUpdate()  // Use FixedUpdate for physics/Rigidbody
    {
        switch (currentState)
        {
            case MovementState.MovingHorizontal:
                // Calculate the direction
                float direction = movingRight ? 1 : -1;

                // Move with Rigidbody2D
                if (rb != null)
                {
                    // Set velocity directly (no accumulation)
                    rb.velocity = new Vector2(direction * moveSpeed, 0);

                    // Check if reached boundary
                    if ((transform.position.x >= rightBound && movingRight) ||
                        (transform.position.x <= leftBound && !movingRight))
                    {
                        // Stop movement
                        rb.velocity = Vector2.zero;

                        // Snap to exact boundary
                        float boundaryX = movingRight ? rightBound : leftBound;
                        transform.position = new Vector3(boundaryX, transform.position.y, transform.position.z);

                        // Switch to pausing state
                        currentState = MovementState.Pausing;
                        pauseTimer = 0f;
                    }
                }
                break;

            case MovementState.Pausing:
                // Ensure velocity is zero during pause
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                }

                // Count time
                pauseTimer += Time.fixedDeltaTime;
                if (pauseTimer >= pauseDuration)
                {
                    // Prepare to move down
                    currentState = MovementState.MovingDown;
                    targetY = transform.position.y - dropDistance;
                }
                break;

            case MovementState.MovingDown:
                if (rb != null)
                {
                    // Move strictly downward with velocity
                    rb.velocity = new Vector2(0, -moveSpeed);

                    // Check if reached target Y
                    if (transform.position.y <= targetY)
                    {
                        // Stop movement
                        rb.velocity = Vector2.zero;

                        // Snap to exact Y position
                        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

                        // Switch direction and return to horizontal movement
                        movingRight = !movingRight;
                        currentState = MovementState.MovingHorizontal;
                    }
                }
                break;
        }
    }
}