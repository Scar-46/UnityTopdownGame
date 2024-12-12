using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float theashold;
    private bool playerDead = false;
    public float smoothSpeed = 6f; // Speed of the camera centering

    private void Awake()
    {
        transform.position = player.position;
    }
    void OnEnable()
    {
        PlayerStats.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        PlayerStats.OnPlayerDeath -= HandlePlayerDeath;
    }

    void Update()
    {
        if (!playerDead)
        {
            // Get the mouse position in world space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the midpoint between the player and mouse position
            Vector3 midpoint = (player.position + mousePos) / 2f;

            // Clamp the midpoint to keep it within a certain range relative to the player
            midpoint.x = Mathf.Clamp(midpoint.x, -theashold + player.position.x, theashold + player.position.x);
            midpoint.y = Mathf.Clamp(midpoint.y, -theashold + player.position.y, theashold + player.position.y);

            // Smoothly move the camera to the midpoint
            transform.position = Vector3.Lerp(transform.position, midpoint, Time.deltaTime * smoothSpeed);
        }
        else
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
}