using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public enum openingDirection
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    public openingDirection opening;
    public float waitToDestroy;

    private RoomTemplates roomTemplates;
    private int nextRoom;
    private bool spawned = false;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, waitToDestroy);
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn()
    {
        switch (opening)
        {
            case openingDirection.TOP:
                Instantiate(roomTemplates.bottomRooms[nextRoom], transform.position, Quaternion.identity);
                break;
            case openingDirection.BOTTOM:
                Instantiate(roomTemplates.topRooms[nextRoom], transform.position, Quaternion.identity);
                break;
            case openingDirection.LEFT:
                Instantiate(roomTemplates.leftRooms[nextRoom], transform.position, Quaternion.identity);
                break;
            case openingDirection.RIGHT:
                Instantiate(roomTemplates.rightRooms[nextRoom], transform.position, Quaternion.identity);
                break;
        }
        spawned = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("SpawnPoint"))
        {
            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //Close Room Goes here.
                Destroy(gameObject);
            }
            spawned = true;

        }
    }
}
