using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public GameObject emptyTilePrefab; // Prefab to replace standardpellet with
    public Transform map; 
    public GameObject lastVisitedTile; // Ignore IsWalkable targeting for current tile 
    private GameObject lastCollisionTile; // Stops repeated collision with same tile when pacman is stuck
    private GameObject lastTeleportExit; // Used to stop teleporting back and forth between teleporters 
    private GameObject spawnPoint; 
    public ParticleSystem walkingParticleSystem;
    public ParticleSystem bumpParticleSystem;
    public GameObject gameManagers;
    private Coroutine loseLifeCoroutine;

    // Managers
    private MapManager mapManager;
    private AudioManager audioManager;
    private HUDManager hudManager; 
    private GhostManager ghostManager;

    // Model
    public int lives = 3;
    public bool onDieCooldown = false; 
    public bool hasGameStarted = false; 


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
        // Get Managers from GameManager
        mapManager = gameManagers.GetComponentInChildren<MapManager>();
        audioManager = gameManagers.GetComponentInChildren<AudioManager>();
        ghostManager = gameManagers.GetComponentInChildren<GhostManager>();
        
        // Find In Scene
        hudManager = GameObject.Find("HUD").GetComponent<HUDManager>();

        walkingParticleSystem.Stop();
        bumpParticleSystem.Stop();

        animator = GetComponent<Animator>();

        spawnPoint = lastVisitedTile; 
    }

    void Update()
    {
        if (!hasGameStarted) { 
            return; 
        }

        if (checkForGhostCollision()) { 
            // If already on die cooldown then dont do anything 
            if (onDieCooldown) { 

            } else {
                teleportToSpawn(); 
            }
            return; 
        } else { 
            onDieCooldown = false; 
        }

        // Get player input
        HandleInput(); 

        // If pacstudent is not lerping
        if (!isLerping) { 
            // If can move in the direction of lastInput
            if (IsWalkable(transform.position + lastInput, lastInput)) {
                currentInput = lastInput;
                // When moving direction is changed reset the collisionTile so it can be hit again
                lastCollisionTile = null;
                bumpParticleSystem.Stop();


                // Naviagte to the next tile
                GameObject gridTile = GetGridTile(transform.position + currentInput);
                if (isTeleporter(gridTile)) {
                    useTeleporter(gridTile);
                } else {
                    Vector3 gridTargetPosition = gridTile.transform.position;
                    StartCoroutine(LerpToPosition(gridTargetPosition));
                }

            } else { 
                // If can move in the direction of currentInput
                if (IsWalkable(transform.position + currentInput, currentInput)) {
                    GameObject gridTile = GetGridTile(transform.position + currentInput);


                    // Naviagte to the next tile
                    if (isTeleporter(gridTile)) {
                        useTeleporter(gridTile);
                    } else {
                        Vector3 gridTargetPosition = gridTile.transform.position;
                        StartCoroutine(LerpToPosition(gridTargetPosition));
                    }
                    
                } else {
                    // Stop moving as both the lastInput and currentInput are not walkable

                    // If stopped moving must of hit something. 
                    GameObject collisionTile = GetGridTile(transform.position + currentInput);
                    if (collisionTile != lastCollisionTile) {
                        // Play bump sound
                        audioManager.PlayPacStudentCollidesWall();
                        lastCollisionTile = collisionTile;

                        // Play bump particle effect 
                        bumpParticleSystem.Play();
                    }
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
        walkingParticleSystem.Play();
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
        walkingParticleSystem.Stop();
        isLerping = false;
    }

    private void onTileVisited() {
        // Updating lastVisited
        lastVisitedTile = GetGridTile(transform.position);

        if (lastVisitedTile == null) { 
            return; 
        } 

        // Play pellet eaten sound
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

            // Add points 
            hudManager.addPoints(10);
        }

        if (lastVisitedTile.name.Contains("powerpellet")) {
            // Change properties to match emptyTilePrefab
            lastVisitedTile.GetComponent<SpriteRenderer>().sprite = emptyTilePrefab.GetComponent<SpriteRenderer>().sprite;
            lastVisitedTile.name = emptyTilePrefab.name;

            // Notify managers
            ghostManager.powerPelletEaten();        
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

    private bool isTeleporter(GameObject tile) { 
        // Don't act as a teleporter if just exited from this teleporter
        if (tile == lastTeleportExit) {
            return false; 
        }
        
        // Check mapManager
        MapManager mapManagerScript = mapManager.GetComponent<MapManager>();
        if (mapManagerScript.leftTeleporters[0] == tile || mapManagerScript.leftTeleporters[1] == tile) {
            return true; 
        } else if (mapManagerScript.rightTeleporters[0] == tile || mapManagerScript.rightTeleporters[1] == tile) {
            return true; 
        }

        return false; 
    }

    private void useTeleporter(GameObject tile) { 
        isLerping = false; 

        MapManager mapManagerScript = mapManager.GetComponent<MapManager>();
        if (mapManagerScript.leftTeleporters[0] == tile) {
            lastTeleportExit = mapManagerScript.rightTeleporters[0];
            transform.position = mapManagerScript.rightTeleporters[0].transform.position;
        } else if (mapManagerScript.leftTeleporters[1] == tile) {
            lastTeleportExit = mapManagerScript.rightTeleporters[1];
            transform.position = mapManagerScript.rightTeleporters[1].transform.position;
        } else if (mapManagerScript.rightTeleporters[0] == tile) {
            lastTeleportExit = mapManagerScript.leftTeleporters[0];
            transform.position = mapManagerScript.leftTeleporters[0].transform.position;
        } else if (mapManagerScript.rightTeleporters[1] == tile) {
            lastTeleportExit = mapManagerScript.leftTeleporters[1];
            transform.position = mapManagerScript.leftTeleporters[1].transform.position;
        }
    }

    private void teleportToSpawn() { 
        isLerping = false; 
        transform.position = spawnPoint.transform.position;

        if (loseLifeCoroutine != null)
        {
            StopCoroutine(loseLifeCoroutine);
        }
        loseLifeCoroutine = StartCoroutine(loseLife());
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

    private IEnumerator loseLife() {
        yield return new WaitForSeconds(0.1f);
        if (onDieCooldown) {
            yield break;
        }
        onDieCooldown = true;

        if (lives == 1) { 
            Die();
            yield break;
        } 

        lives--;
        hudManager.removeLife();

        // Reset lastVisitedTile
        lastVisitedTile = spawnPoint;

        // Reset lastCollisionTile
        lastCollisionTile = null;

        // Reset lastTeleportExit
        lastTeleportExit = null;

        // Reset lastInput
        lastInput = Vector3.zero;

        // Reset currentInput
        currentInput = Vector3.zero;

        // Reset isLerping
        isLerping = false;

        // Reset animator
        animator.SetInteger("Direction", 0);

        // Reset walkingParticleSystem
        walkingParticleSystem.Stop();

        // Reset bumpParticleSystem
        bumpParticleSystem.Stop();

        transform.position = spawnPoint.transform.position;
        Debug.Log(transform.position + " " + spawnPoint.transform.position);

        loseLifeCoroutine = null;
        onDieCooldown = false;
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

    private bool checkForGhostCollision()
    {
        // Check if pacman is colliding with any of the ghosts
        GameObject[] ghosts = mapManager.ghosts;
        for (int i = 0; i < ghosts.Length; i++)
        {
            if (GetComponent<Collider2D>().IsTouching(ghosts[i].GetComponent<Collider2D>()))
            {
                return true; 
            }
        }
        return false; 
    }



}