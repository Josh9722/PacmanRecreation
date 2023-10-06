using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Animator animator;

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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetInteger("Direction", 2);
            // TODO
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetInteger("Direction", 1);
            // TODO
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetInteger("Direction", 3);
            // TODO
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("Direction", 4);
            // TODO
        }
        
    }

    // Example of setting the IsDead parameter:
    // This can be triggered by other events, such as collision with ghosts.
    public void Die()
    {
        animator.SetBool("IsDead", true);
    }
}