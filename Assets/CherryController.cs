using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab; // Reference to the cherry prefab
    public float spawnInterval = 10.0f; // Time between cherry spawns in seconds
    private float moveSpeed = 1.0f; // Cherry movement speed
    private float timer;
    private Camera mainCamera;

    private void Start()
    {
        timer = spawnInterval;
        mainCamera = Camera.main;

        // Start spawning cherries
        StartCoroutine(SpawnCherries());
    }

    private IEnumerator SpawnCherries()
    {
        while (true)
        {
            yield return new WaitForSeconds(timer);

            // Calculate a random position outside of the camera view
            Vector3 spawnPosition = CalculateRandomSpawnPosition();

            // Instantiate the cherry at the spawn position
            GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

            // Move the cherry
            StartCoroutine(MoveCherry(cherry));

            // Reset the timer for the next spawn
            timer = spawnInterval;
        }
    }

    private Vector3 CalculateRandomSpawnPosition()
    {
        // Get the camera's viewport dimensions
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Define spawn positions outside of the camera view (top, bottom, left, right)
        Vector3[] spawnPositions = new Vector3[]
        {
            new Vector3(Random.Range(-cameraWidth / 2f, cameraWidth / 2f), cameraHeight / 2f + 20f, 0f), // Top
            new Vector3(Random.Range(-cameraWidth / 2f, cameraWidth / 2f), -cameraHeight / 2f - 20f, 0f), // Bottom
            new Vector3(-cameraWidth / 2f - 20f, Random.Range(-cameraHeight / 2f, cameraHeight / 2f), 0f), // Left
            new Vector3(cameraWidth / 2f + 20f, Random.Range(-cameraHeight / 2f, cameraHeight / 2f), 0f) // Right
        };

        // Choose a random spawn position
        int randomIndex = Random.Range(0, spawnPositions.Length);
        return spawnPositions[randomIndex];
    }


    private IEnumerator MoveCherry(GameObject cherry)
    {
        Vector3 targetPosition = new Vector3(-cherry.transform.position.x, -cherry.transform.position.y, cherry.transform.position.z);
        float journeyLength = Vector3.Distance(cherry.transform.position, targetPosition);

        float t = 0; // Start at 0
        while (Vector3.Distance(cherry.transform.position, targetPosition) > 0.01f) // Use a small threshold value
        {
            // Increment t based on the move speed and delta time
            t += moveSpeed * Time.deltaTime / journeyLength;

            // Calculate the new position of the cherry
            Vector3 newPosition = Vector3.Lerp(cherry.transform.position, targetPosition, t);
            cherry.transform.position = newPosition;

            yield return null;
        }

        // Destroy!
        if (cherry != null)
        {
            // Exterminate...Exterminate
            Destroy(cherry);
        }
    }




}
