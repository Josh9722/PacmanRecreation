using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateShowcase : MonoBehaviour
{
    private Animator animator;
    private int currentState = 1; // 1 = alive, 2 = dead, 3 = scared

    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(StartChangeStateAfterDelay(3)); // Starting the coroutine after 9 seconds
    }

    IEnumerator StartChangeStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ChangeState());
    }


    IEnumerator ChangeState()
    {
        while (true)
        {
            
            if (currentState == 1) {
                // Alive 
                animator.SetBool("IsDead", false);

                if (gameObject.name != "PacStudentStateChanger")
                {
                    animator.SetBool("IsScared", false);
                }
            } else if (currentState == 2) {
                // Dead
                animator.SetBool("IsDead", true);

                if (gameObject.name != "PacStudentStateChanger")
                {
                    animator.SetBool("IsScared", false);
                }
            } else if (currentState == 3) {
                // Scared
                animator.SetBool("IsDead", false);
                animator.SetBool("IsScared", true);
            }

            currentState++;
            
            if (gameObject.name == "PacStudentStateChanger" && currentState > 2)
            {
                currentState = 1;
            }
            else 
            {
                if (currentState > 3)
                {
                    currentState = 1;
                }
            }

            
            yield return new WaitForSeconds(3);
        }
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);
    }

    public void Scared()
    {
        animator.SetBool("IsScared", true);
    }
}
