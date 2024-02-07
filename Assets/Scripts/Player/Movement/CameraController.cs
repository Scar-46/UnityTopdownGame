using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float theashold;


    private void Awake()
    {
        transform.position = player.position;
    }

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the midpoint between the player and mouse position
        Vector3 midpoint = (player.position + mousePos) / 2f;

        midpoint.x = Mathf.Clamp(midpoint.x, -theashold + player.position.x, theashold + player.position.x);
        //midpoint.y = player.position.y;
        midpoint.y = Mathf.Clamp(midpoint.y, -theashold + player.position.y, theashold + player.position.y);

        // Set the Cinemachine Virtual Camera position to the calculated midpoint
        transform.position = midpoint;
    }
}