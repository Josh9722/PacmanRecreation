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
            // As pacstudent does not have a scared state
            if (gameObject.name == "PacStudentStateChanger") { 
                if (currentState == 1) {
                    // Alive 
                    animator.SetBool("IsDead", false);
                } else if (currentState == 2) {
                    // Dead
                    animator.SetBool("IsDead", true);
                } 
            }
            else { 
                if (currentState == 1) { 
                    // Alive 
                    animator.SetBool("IsDead", false);
                    animator.SetBool("IsRecovery", false);
                    animator.SetBool("IsScared", false);
                } else if (currentState == 2) {
                    // Scared
                    animator.SetBool("IsDead", false);
                    animator.SetBool("IsRecovery", false);
                    animator.SetBool("IsScared", true);
                } else if (currentState == 3) {
                    // Recovery
                    animator.SetBool("IsDead", false);
                    animator.SetBool("IsScared", false);
                    animator.SetBool("IsRecovery", true);
                } else if (currentState == 4) { 
                    animator.SetBool("IsRecovery", false);
                    animator.SetBool("IsScared", false);
                    animator.SetBool("IsDead", true);
                }
            }

            // State incrementation
            currentState++;            
            if (gameObject.name == "PacStudentStateChanger" && currentState > 2)
            {
                currentState = 1;
            }
            else 
            {
                if (currentState > 4)
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
