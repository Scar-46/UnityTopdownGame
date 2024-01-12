using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float smothing = 0.6f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        if(player != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, player.position + offset, smothing);
            transform.position = newPosition;
        }

    }
}
