using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    // ****** Movement Specific ******
    public float speed = 5f;
    private Vector3 lastInputDirection = Vector3.zero;



    // ****** Animator ****** 
    private Animator animator;

    // Translating direction to animator parameters
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
        //HandleCharacterMovement();

        // Update animation
        HandleMovementAnimation(); 

        Debug.Log(lastInputDirection); 
    }


    // ****** MOVEMENT SPECIFIC Specific ******
    private void HandleInput() {
        // Gather player input (W, A, S, D keys).
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Store the last input direction if the player provides more input.
        if (horizontalInput != 0 || verticalInput != 0)
        {
            lastInputDirection = new Vector3(horizontalInput, verticalInput, 0f);
        }
    }


    // ****** ANIMATOR SPECIFIC ******
    void HandleMovementAnimation()
    {
        // Set currentAnimationDirection basesd on lastInputDirection
        if (lastInputDirection.x > 0)
        {
            animator.SetInteger("Direction", RIGHT);
        }
        else if (lastInputDirection.x < 0)
        {
            animator.SetInteger("Direction", LEFT);
        }
        else if (lastInputDirection.y > 0)
        {
            animator.SetInteger("Direction", UP);
        }
        else if (lastInputDirection.y < 0)
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