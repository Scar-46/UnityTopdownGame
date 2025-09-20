using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public float threshold;
    public float smoothSpeed = 6f; // Speed of the camera centering
    private Transform player;
    private bool playerDead = false;

    private void OnEnable()
    {
        PlayerStats.OnPlayerDeath += HandlePlayerDeath;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDeath -= HandlePlayerDeath;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (player != null && !playerDead)
        {
            // Get the mouse position in world space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the midpoint between the player and mouse position
            Vector3 midpoint = (player.position + mousePos) / 2f;

            // Clamp the midpoint to keep it within a certain range relative to the player
            midpoint.x = Mathf.Clamp(midpoint.x, -threshold + player.position.x, threshold + player.position.x);
            midpoint.y = Mathf.Clamp(midpoint.y, -threshold + player.position.y, threshold + player.position.y);

            // Smoothly move the camera to the midpoint
            transform.position = Vector3.Lerp(transform.position, midpoint, Time.deltaTime * smoothSpeed);
        }
        else if (player != null)
        {
            // Smoothly center the camera on the player
            Vector3 targetPosition = player.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
        }
    }

    private void HandlePlayerDeath()
    {
        playerDead = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindPlayer());
    }

    private IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerDead = false;
            transform.position = player.position;
        }
        else
        {
            Debug.LogWarning("Player not found in the scene. Ensure the player object is tagged 'Player'.");
        }
    }
}
