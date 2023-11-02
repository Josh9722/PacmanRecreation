using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public GameObject emptyTilePrefab; // Prefab to replace standardpellet with
    public Transform map; 
    public GameObject lastVisitedTile; // Ignore targeting for current tile 
    public ParticleSystem dustParticleSystem;


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
        dustParticleSystem.Stop();
        animator = GetComponent<Animator>();
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
                if (IsWalkable(transform.position + currentInput, currentInput)) {
                    GameObject gridTile = GetGridTile(transform.position + currentInput);
                    Vector3 gridTargetPosition = gridTile.transform.position;
                    StartCoroutine(LerpToPosition(gridTargetPosition));
                } else {
                    // Stop moving as both the lastInput and currentInput are not walkable
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
        dustParticleSystem.Play();
        float journeyLength = Vector3.Distance(transform.position, targetPos);
        float startTime = Time.time;
        float journeyTime = journeyLength / speed;
        float journeyProgress = 0f;
        Vector3 initialPosition = transform.position;

        while (journeyProgress < 1.0f)
        {
            journeyProgress += Time.deltaTime / journeyTime;
            transform.position = Vector3.Lerp(initialPosition, targetPos, journeyProgress);
            yield return null;
        }

        onTileVisited();
        dustParticleSystem.Stop();
        isLerping = false;
    }

    private void onTileVisited() {
        // Updating lastVisited
        lastVisitedTile = GetGridTile(transform.position);

        // Play pellet eaten sound
        // find gameobject named AudioManager
        GameObject audioManagerObject = GameObject.Find("AudioManager");
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
        if (lastVisitedTile.name.Contains("pellet")) {
            audioManager.PlayPelletEaten();
        } else { 
            audioManager.PlayPacStudentMove();
        }
            

        // Removing pellets
        if (lastVisitedTile.name.Contains("standardpellet")) {
            // Change properties to match emptyTilePrefab
            lastVisitedTile.GetComponent<SpriteRenderer>().sprite = emptyTilePrefab.GetComponent<SpriteRenderer>().sprite;
            lastVisitedTile.name = emptyTilePrefab.name;
        }

        if (lastVisitedTile.name.Contains("powerpellet")) {
            // Change properties to match emptyTilePrefab
            lastVisitedTile.GetComponent<SpriteRenderer>().sprite = emptyTilePrefab.GetComponent<SpriteRenderer>().sprite;
            lastVisitedTile.name = emptyTilePrefab.name;
        }

        
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
                    if (image.gameObject == lastVisitedTile)  
                    {
                        continue; // Don't include current tile in search
                    }
                    
                    // Exclude tiles that can't be walked on (i.e walls)
                    if (image.gameObject.name.Contains("corner") || image.gameObject.name.Contains("wall") || image.gameObject.name.Contains("junction"))
                    {
                        continue;
                    }
 
                    Vector3 compareDistance = image.position; 
                    float leeway = 0.1f;
                    // If going Left or Right, tile must match y axis
                    if (direction.y == 0)
                    {
                        if (Mathf.Abs(compareDistance.y - position.y) > leeway)
                        {
                            continue;
                        }
                    }
                    // If going Up or Down, tile must match x axis
                    else if (direction.x == 0)
                    {
                        if (Mathf.Abs(compareDistance.x - position.x) > leeway)
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
                        continue; // Don't include current tile in search
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