using UnityEngine;
using UnityEngine.Tilemaps;

public enum Entry
{
    TOP,
    BOTTOM,
    LEFT,
    RIGHT,
    NONE
}
public class TilemapSize : MonoBehaviour
{
    public Entry[] spawns;
    public Entry previous;

    private Tilemap currentTilemap;
    private RoomTemplates roomTemplates;
    private GameObject newRoom;
    private GameObject oldRoom;

    public bool spawned = false;
    public bool bossRoom = false;

    void Awake()
    {
        //Get all the posible rooms.
        roomTemplates = GameObject.FindGameObjectWithTag("RoomSpawner").GetComponent<RoomTemplates>();
        currentTilemap = gameObject.GetComponentInChildren<Tilemap>();
        currentTilemap.CompressBounds();
    }

    private void Start()
    {
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        spawned = true;
        int nextRoom;
        foreach (var room in spawns)
        {
            if (roomTemplates.roomCounter <= roomTemplates.maxRooms && room != previous)
            {
                switch (room)
                {
                    case Entry.TOP:

                        nextRoom = Random.Range(0, roomTemplates.bottomRooms.Length - 1);
                        GetNextRoom(roomTemplates.bottomRooms, nextRoom, 1, room);
                        newRoom.GetComponent<TilemapSize>().previous = Entry.BOTTOM;
                        break;

                    case Entry.BOTTOM:

                        nextRoom = Random.Range(0, roomTemplates.topRooms.Length - 1);

                        GetNextRoom(roomTemplates.topRooms, nextRoom, -1, room);
                        newRoom.GetComponent<TilemapSize>().previous = Entry.TOP;
                        break;

                    case Entry.LEFT:

                        nextRoom = Random.Range(0, roomTemplates.rightRooms.Length - 1);
                        GetNextRoom(roomTemplates.rightRooms, nextRoom, -1, room);
                        newRoom.GetComponent<TilemapSize>().previous = Entry.RIGHT;
                        break;

                    case Entry.RIGHT:

                        nextRoom = Random.Range(0, roomTemplates.leftRooms.Length - 1);
                        GetNextRoom(roomTemplates.leftRooms, nextRoom, 1, room);
                        newRoom.GetComponent<TilemapSize>().previous = Entry.LEFT;
                        break;
                }
                newRoom.GetComponent<TilemapSize>().oldRoom = gameObject;
                roomTemplates.roomCounter++;
            }
            else if (room != previous)
            {
                if (roomTemplates.bossSpawned == false)
                {
                    SpawnBoss(room);
                }
                else
                {
                    CloseRoom();
                }

            }
        }
    }
    private void SpawnBoss(Entry room)
    {
        roomTemplates.bossSpawned = true;
        Debug.Log("Boss");
        switch (room)
        {
            case Entry.TOP:

                GetNextRoom(roomTemplates.bossRooms, 0, 1, room);
                break;

            case Entry.BOTTOM:

                GetNextRoom(roomTemplates.bossRooms, 1, -1, room);
                break;

            case Entry.LEFT:

                GetNextRoom(roomTemplates.bossRooms, 2, -1, room);
                break;

            case Entry.RIGHT:

                GetNextRoom(roomTemplates.bossRooms, 3, 1, room);
                break;
        }
    }

    private void GetNextRoom(GameObject[] rooms, int index, int sign, Entry TB)
    {
        Tilemap nextTilemap = rooms[index].GetComponentInChildren<Tilemap>();
        nextTilemap.CompressBounds();
        Vector3 nextPosition = this.transform.position;
        if (TB == Entry.BOTTOM)
        {
            nextPosition.y = nextPosition.y + ((nextTilemap.size.y + 1) * sign);
            nextPosition.x = nextPosition.x + Mathf.Floor((nextTilemap.size.x - currentTilemap.size.x) / 2);
        }
        else if (TB == Entry.TOP)
        {
            nextPosition.y = nextPosition.y + ((currentTilemap.size.y + 1) * sign);
            nextPosition.x = nextPosition.x + Mathf.Floor((nextTilemap.size.x - currentTilemap.size.x) / 2);
        }
        else if (TB == Entry.LEFT)
        {
            nextPosition.x = nextPosition.x + (currentTilemap.size.x * sign);
        }
        else if(TB == Entry.RIGHT)
        {
            nextPosition.x = nextPosition.x + (nextTilemap.size.x * sign);
        }
        newRoom = Instantiate(rooms[index], nextPosition, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy every room created over another room.
        if (collision.CompareTag("Rooms"))
        {
            if (collision.GetComponent<TilemapSize>().spawned == true || (collision.GetComponent<TilemapSize>().spawned == false && spawned == false))
            {
                roomTemplates.roomCounter--;
                if(oldRoom != null)
                {
                    //oldRoom.GetComponent<TilemapSize>().CloseRoom();
                }

                if (bossRoom)
                {
                    roomTemplates.bossDestroy = true;
                }

                Destroy(gameObject);
            }
        }
    }

    public void CloseRoom()
    {
        Debug.Log("ChangigRoom");
        switch (previous)
        {
            case Entry.TOP:
                Debug.Log("T" + (roomTemplates.topRooms.Length - 1));
                newRoom = Instantiate(roomTemplates.topRooms[roomTemplates.topRooms.Length - 1], transform.position, Quaternion.identity);
                newRoom.GetComponent<TilemapSize>().spawned = true;
                Destroy(gameObject);
                break;


            case Entry.BOTTOM:
                Debug.Log("B" + (roomTemplates.bottomRooms.Length - 1));
                newRoom = Instantiate(roomTemplates.bottomRooms[roomTemplates.bottomRooms.Length - 1], transform.position, Quaternion.identity);
                newRoom.GetComponent<TilemapSize>().spawned = true;
                Destroy(gameObject);
                break;


            case Entry.LEFT:
                Debug.Log("L" + (roomTemplates.leftRooms.Length - 1));
                newRoom = Instantiate(roomTemplates.leftRooms[roomTemplates.leftRooms.Length - 1], transform.position, Quaternion.identity);
                newRoom.GetComponent<TilemapSize>().spawned = true;
                Destroy(gameObject);
                break;

            case Entry.RIGHT:
                Debug.Log("R" + (roomTemplates.rightRooms.Length - 1) );
                newRoom = Instantiate(roomTemplates.rightRooms[roomTemplates.rightRooms.Length - 1], transform.position, Quaternion.identity);
                newRoom.GetComponent<TilemapSize>().spawned = true;
                Destroy(gameObject);
                break;
        }
    }
}
