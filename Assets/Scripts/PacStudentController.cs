using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Animator animator;

    private const int UP = 2;
    private const int DOWN = 1;
    private const int LEFT = 3;
    private const int RIGHT = 4;

    private int currentDirection = UP;  // Starting direction
    private float speed = 5.0f;  // Movement speed,

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {

        switch (currentDirection)
        {
            case UP:
                MoveUp();
                // TODO: Add condition to change direction to RIGHT
                break;
            case DOWN:
                MoveDown();
                // TODO: Add condition to change direction to LEFT
                break;
            case LEFT:
                MoveLeft();
                // TODO: Add condition to change direction to UP
                break;
            case RIGHT:
                MoveRight();
                // TODO: Add condition to change direction to DOWN
                break;
        }

        senseDirectionChange(); // Check for direction change
    }

    private void senseDirectionChange()
    {
        // Fetch the current position of PacStudent
        Vector3 currentPosition = transform.position;

        // Check conditions to change direction based on PacStudent's position
        if (currentDirection == RIGHT && IsCloseTo(currentPosition, -53.3f, 84f))
        {
            currentDirection = DOWN;
        }
        else if (currentDirection == DOWN && IsCloseTo(currentPosition, -53.3f, 60f))
        {
            currentDirection = LEFT;
        }
        else if (currentDirection == LEFT && IsCloseTo(currentPosition, -84f, 60f))
        {
            currentDirection = UP;
        }
        else if (currentDirection == UP && IsCloseTo(currentPosition, -84f, 84f))
        {
            Debug.Log("TURN RIGHT");
            currentDirection = RIGHT;
        }
    }


    private bool IsCloseTo(Vector3 currentPosition, float targetX, float targetY, float range = 1f)
    {
        return Mathf.Abs(currentPosition.x - targetX) <= range && Mathf.Abs(currentPosition.y - targetY) <= range;
    }



    private void MoveUp()
    {
        animator.SetInteger("Direction", UP);
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    private void MoveDown()
    {
        animator.SetInteger("Direction", DOWN);
        transform.Translate(transform.up * -speed * Time.deltaTime);
    }

    private void MoveLeft()
    {
        animator.SetInteger("Direction", LEFT);
        transform.Translate(transform.right * speed * Time.deltaTime);
    }

    private void MoveRight()
    {
        animator.SetInteger("Direction", RIGHT);
        transform.Translate(transform.right * -speed * Time.deltaTime);
    }



    // Example of setting the IsDead parameter:
    // This can be triggered by other events, such as collision with ghosts.
    public void Die()
    {
        animator.SetBool("IsDead", true);
    }
}