using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseGhosts : MonoBehaviour
{
    private Animator animator;
    private int currentDirection = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
    
        StartCoroutine(StartChangeDirectionAfterDelay(3)); // Starting the coroutine after 9 seconds
    }

    IEnumerator StartChangeDirectionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ChangeDirection());
    }


    IEnumerator ChangeDirection()
    {
        while (true)
        {
            animator.SetInteger("Direction", currentDirection);

            currentDirection++;
            if (currentDirection > 4)
            {
                currentDirection = 1;
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
