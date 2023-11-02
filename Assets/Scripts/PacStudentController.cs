using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    // ****** Movement Specific Members ******
    public float speed = 4f;
    private Vector3 lastInput = Vector3.zero;
    private Vector3 currentInput;
    
    // Lerping Variables
    private bool isMoving = false;
    private float startTime;
    private float journeyLength;


    // ****** Animator Specific Members ****** 
    private Animator animator;
    private const int UP = 2;
    private const int DOWN = 1;
    private const int LEFT = 3;
    private const int RIGHT = 4;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get player input
        HandleInput(); 

        // Update character position 
        HandleCharacterMovement();

        // Update animation
        HandleMovementAnimation(); 
    }


    // ****** MOVEMENT SPECIFIC ******
    private void HandleInput() {
        // Gather player input (W, A, S, D keys).
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Store the last input direction if the player provides more input.
        if (horizontalInput != 0 || verticalInput != 0)
        {
            lastInput = new Vector3(horizontalInput, verticalInput, 0f);
        }
    }

    private void HandleCharacterMovement() {
        // If not lerping, move PacStudent to the target position
        if (!isMoving)
        {
            TryMove(lastInput);
        }

        LerpMovement(); 
    }

    private void TryMove(Vector3 direction)
    {
        // Calculate the target position based on the input direction
        currentInput = transform.position + direction;

        // Check if the target position is walkable (e.g., not a wall)
        if (IsWalkable(currentInput))
        {
            isMoving = true;
            startTime = Time.time;
            journeyLength = Vector3.Distance(transform.position, currentInput);
        }
    }

    private void LerpMovement()
    {
        if (isMoving)
        {
            float journeyLength = Vector3.Distance(transform.position, currentInput);
            float journeyTime = journeyLength / speed;

            // Calculate the journey fraction based on time
            float journeyFraction = (Time.time - startTime) / journeyTime;
            if (float.IsNaN(journeyFraction)) {
                journeyFraction = 0f;
            } 

            // Ensure journeyFraction is within [0, 1]
            journeyFraction = Mathf.Clamp01(journeyFraction);

            // Lerp PacStudent's position
            transform.position = Vector3.Lerp(transform.position, currentInput, journeyFraction);

            // Check if PacStudent has reached the target position
            if (journeyFraction >= 1.0f)
            {
                isMoving = false;
                // TODO: Stop PacStudent's movement audio and animation here
            }
        }
    }



    private bool IsWalkable(Vector3 position)
    {
        return true; 
    }




    // ****** ANIMATOR SPECIFIC ******
    void HandleMovementAnimation()
    {
        // Set currentAnimationDirection basesd on lastInput
        if (lastInput.x > 0)
        {
            animator.SetInteger("Direction", RIGHT);
        }
        else if (lastInput.x < 0)
        {
            animator.SetInteger("Direction", LEFT);
        }
        else if (lastInput.y > 0)
        {
            animator.SetInteger("Direction", UP);
        }
        else if (lastInput.y < 0)
        {
            animator.SetInteger("Direction", DOWN);
        }
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);
    }

    // ****** HELPER FUNCTIONS ******
    private bool IsCloseTo(Vector3 currentPosition, float targetX, float targetY, float range = 1f)
    {
        return Mathf.Abs(currentPosition.x - targetX) <= range && Mathf.Abs(currentPosition.y - targetY) <= range;
    }



    // Example of setting the IsDead parameter:
    // This can be triggered by other events, such as collision with ghosts.
    
}