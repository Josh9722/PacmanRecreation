using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public Transform map; 
    public GameObject lastVisitedTile; // Ignore targeting for current tile 


    // ****** Movement Specific Members ******
    public float speed = 4f;
    private Vector3 lastInput = Vector3.zero;
    private Vector3 currentInput;
    
    // Lerping Variables
    private bool isLerping = false;


    // ****** Animator Specific Members ****** 
    private Animator animator;
    private const int UP = 2;
    private const int DOWN = 1;
    private const int LEFT = 3;
    private const int RIGHT = 4;


    void Start()
    {
        animator = GetComponent<Animator>();

        // Starting top left corner
        //transform.position = lastVisitedTile.transform.position;
    }

    void Update()
    {
        // Get player input
        HandleInput(); 

        // If pacstudent is not lerping
        if (!isLerping) { 
            // If can move in the direction of lastInput
            if (IsWalkable(transform.position + lastInput, lastInput)) {
                currentInput = lastInput;

                // Lerp to the next tile
                GameObject gridTile = GetGridTile(transform.position + currentInput);
                Vector3 gridTargetPosition = gridTile.transform.position;
                StartCoroutine(LerpToPosition(gridTargetPosition));
            } else { 
                // If can move in the direction of currentInput
                if (IsWalkable(transform.position + currentInput, lastInput)) {
                    // Lerp to the next tile
                } else {
                    // Stop moving
                }
            }
        }

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

    private IEnumerator LerpToPosition(Vector3 targetPos)
    {
        isLerping = true;
        float journeyLength = Vector3.Distance(transform.position, targetPos);
        float startTime = Time.time;

        while (Time.time < startTime + journeyLength / speed)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, targetPos, fractionOfJourney);
            yield return null;
        }

        transform.position = targetPos;
        lastVisitedTile = GetGridTile(transform.position);
        isLerping = false;
    }


    private bool IsWalkable(Vector3 position, Vector3 direction)
    {
        // Iterate through all objects in the map
        foreach (Transform quadrant in map.transform)
        {
            foreach (Transform row in quadrant)
            {
                foreach (Transform image in row)
                {
                    if (!image.gameObject.name.Contains("standardpellet") || image.gameObject == lastVisitedTile)
                    {
                        continue; // Skip this tile if it is not a standard pellet
                    }
 
                    Vector3 compareDistance = image.position;
                    // If going Left or Right, tile must match y axis 
                    if (direction.y == 0)
                    {
                        if (compareDistance.y != position.y)
                        {
                            continue;
                        }
                    }
                    // If going Up or Down, tile must match x axis
                    else if (direction.x == 0)
                    {
                        if (compareDistance.x != position.x)
                        {
                            continue;
                        }
                    } 

                    float distanceThreshold = 6f; // Adjust this value as needed
                    if (IsCloseTo(position, compareDistance.x, compareDistance.y, distanceThreshold))
                    {
                        //Debug.Log("Can Walkj on " + image.gameObject.name + " at " + image.position + " from " + position + " with threshold " + distanceThreshold + " and direction " + direction);
                        return true; // Tile found at the given position
                    }
                }
            }
        }

        return false; // No tile found at the given position
    }

    private GameObject GetGridTile(Vector3 position)
    {
        // Iterate through all objects in the map
        foreach (Transform quadrant in map.transform)
        {
            foreach (Transform row in quadrant)
            {
                foreach (Transform image in row)
                {
                    if (image.gameObject == lastVisitedTile)
                    {
                        continue; // Skip this tile if it is not a standard pellet
                    }

                    // Find position of middle of tile (i.e subtract width) 
                    Vector3 middleOfTile = image.position;

                    float distanceThreshold = 6f; // Adjust this value as needed
                    if (IsCloseTo(position, middleOfTile.x, middleOfTile.y, distanceThreshold))
                    {
                        return image.gameObject; 
                    }
                }
            }
        }

        return null; // No tile found at the given position
    }




    // ****** ANIMATOR SPECIFIC ******
    void HandleMovementAnimation()
    {
        // Set currentAnimationDirection basesd on lastInput
        if (currentInput.x > 0)
        {
            animator.SetInteger("Direction", RIGHT);
        }
        else if (currentInput.x < 0)
        {
            animator.SetInteger("Direction", LEFT);
        }
        else if (currentInput.y > 0)
        {
            animator.SetInteger("Direction", UP);
        }
        else if (currentInput.y < 0)
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