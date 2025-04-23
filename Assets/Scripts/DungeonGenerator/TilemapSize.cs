
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class TilemapSize : MonoBehaviour
{
    public Vector2Int[] spawns;
    public bool[] nextSpawned;

    public Vector2Int previous;

    private Tilemap currentTilemap;
    private RoomTemplates roomTemplates;
    private GameObject newRoom;
    private GameObject oldRoom;

    public bool spawned = false;
    public bool bossRoom = false;

    public float offsetX = 0f;
    public float offsetY = 0f;

    //Close Doors

    public int startX = 0;
    public int startY = 0;

    public Tilemap upperWalls;
    public Tilemap middleWalls;
    public Tilemap lowerWalls;
    public Tilemap floor;

    public TileBase LTile;
    public TileBase RTile;
    public TileBase DTile;
    public TileBase T1Tile;
    public TileBase T2Tile;

    void Awake()
    {
        //Get all the posible rooms.
        roomTemplates = GameObject.FindGameObjectWithTag("RoomSpawner").GetComponent<RoomTemplates>();
        currentTilemap = gameObject.GetComponentInChildren<Tilemap>();
        currentTilemap.CompressBounds();
        nextSpawned = new bool[spawns.Length];
    }

    private void Start()
    {
        for (int i = 0; i < spawns.Length; i++)
        {
            if (spawns[i] == previous)
            {
                nextSpawned[i] = true;
            }
        }

        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        spawned = true;
        int nextRoom;

        if (oldRoom != null)
        {
            oldRoom.GetComponent<TilemapSize>().SetSpawnsTrue(previous);
        }

        foreach (var room in spawns)
        {
            if (roomTemplates.roomCounter <= roomTemplates.maxRooms && room != previous)
            {

                if (room == Vector2Int.up)
                {
                    nextRoom = Random.Range(0, roomTemplates.bottomRooms.Length - 1);
                    GetNextRoom(roomTemplates.bottomRooms, nextRoom, room);
                    newRoom.GetComponent<TilemapSize>().previous = Vector2Int.down;
                }
                else if (room == Vector2Int.down)
                {
                    nextRoom = Random.Range(0, roomTemplates.topRooms.Length - 1);
                    GetNextRoom(roomTemplates.topRooms, nextRoom, room);
                    newRoom.GetComponent<TilemapSize>().previous = Vector2Int.up;
                }
                else if (room == Vector2Int.right)
                {
                    nextRoom = Random.Range(0, roomTemplates.leftRooms.Length - 1);
                    GetNextRoom(roomTemplates.leftRooms, nextRoom, room);
                    newRoom.GetComponent<TilemapSize>().previous = Vector2Int.left;
                }
                else if (room == Vector2Int.left)
                {
                    nextRoom = Random.Range(0, roomTemplates.rightRooms.Length - 1);
                    GetNextRoom(roomTemplates.rightRooms, nextRoom, room);
                    newRoom.GetComponent<TilemapSize>().previous = Vector2Int.right;
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

            }
        }
    }

    private void SpawnBoss(Vector2Int room)
    {
        roomTemplates.bossSpawned = true;

        if (room == Vector2Int.up)
        {
            GetNextRoom(roomTemplates.bossRooms, 0, room);
        }
        else if (room == Vector2Int.down)
        {
            GetNextRoom(roomTemplates.bossRooms, 1, room);
        }
        else if (room == Vector2Int.left)
        {
            GetNextRoom(roomTemplates.bossRooms, 2, room);
        }
        else if (room == Vector2Int.right)
        {
            GetNextRoom(roomTemplates.bossRooms, 3, room);
        }

        Vector2Int inverseRoom = new Vector2Int(-room.x, -room.y);
        SetSpawnsTrue(inverseRoom);
    }

    private void GetNextRoom(GameObject[] rooms, int index, Vector2Int direction)
    {
        Tilemap nextTilemap = rooms[index].GetComponentInChildren<Tilemap>();
        nextTilemap.CompressBounds();
        Vector3 nextPosition = this.transform.position;
        float newOffsetY = rooms[index].GetComponent<TilemapSize>().offsetY;
        float newOffsetX = rooms[index].GetComponent<TilemapSize>().offsetX;

        if (direction == Vector2Int.up)
        {
            nextPosition.y = nextPosition.y + ((currentTilemap.size.y) * direction.y) + newOffsetY;
            nextPosition.x = nextPosition.x + Mathf.Floor((nextTilemap.size.x - currentTilemap.size.x) / 2) + newOffsetX;
        }
        else if (direction == Vector2Int.down)
        {
            nextPosition.y = nextPosition.y + ((nextTilemap.size.y) * direction.y)+ newOffsetY;
            nextPosition.x = nextPosition.x + Mathf.Floor((nextTilemap.size.x - currentTilemap.size.x) / 2) + newOffsetX;
        }
        else if (direction == Vector2Int.right)
        {
            nextPosition.y += newOffsetY;
            nextPosition.x = nextPosition.x + (nextTilemap.size.x * direction.x) + newOffsetX;
        }
        else if (direction == Vector2Int.left)
        {
            nextPosition.y += newOffsetY;
            nextPosition.x = nextPosition.x + (currentTilemap.size.x * direction.x) + newOffsetX;
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
                if (bossRoom)
                {
                    roomTemplates.bossDestroy = true;
                }
                Destroy(gameObject);
            }
        }
    }

    public void RemoveDoors()
    {
        for (int i = 0; i < spawns.Length; i++)
        {
            Vector3Int position;
            if (nextSpawned[i] == false)
            {
                if (spawns[i] == Vector2Int.up)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        position = new Vector3Int(-10 - j, startY, 0);
                        upperWalls.SetTile(position, T1Tile);
                        position = new Vector3Int(-10 - j, startY + 1, 0);
                        upperWalls.SetTile(position, T2Tile);
                        floor.SetTile(position, null);
                        middleWalls.SetTile(position, null);
                    }
                    Transform door = gameObject.transform.Find("StartPoints").transform.Find("Point (T)");

                    if (door != null)
                    {
                        Destroy(door.gameObject);
                    }
                }
                else if (spawns[i] == Vector2Int.down)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        position = new Vector3Int(-10 - j, 0, 0);
                        floor.SetTile(position, null);
                        lowerWalls.SetTile(position, DTile);
                    }
                    Transform door = gameObject.transform.Find("StartPoints").transform.Find("Point (B)");

                    if (door != null)
                    {
                        Destroy(door.gameObject);
                    }
                }
                else if (spawns[i] == Vector2Int.right)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        position = new Vector3Int(-1, 5 + j, 0);
                        floor.SetTile(position, null);
                        lowerWalls.SetTile(position, null);
                        lowerWalls.SetTile(position, null);
                        lowerWalls.SetTile(position, RTile);
                    }
                    Transform door = gameObject.transform.Find("StartPoints").transform.Find("Point (R)");

                    if (door != null)
                    {
                        Destroy(door.gameObject);
                    }
                }
                else if (spawns[i] == Vector2Int.left)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        position = new Vector3Int(-startX, 5 + j, 0);
                        floor.SetTile(position, null);
                        lowerWalls.SetTile(position, null);
                        lowerWalls.SetTile(position, LTile);
                    }
                    Transform door = gameObject.transform.Find("StartPoints").transform.Find("Point (L)");

                    if (door != null)
                    {
                        Destroy(door.gameObject);
                    }

                }
            }
        }
    }
    
    public void SetSpawnsTrue( Vector2Int direction)
    {
        for (int i = 0; i < spawns.Length; i++)
        {
            if (spawns[i] == new Vector2Int(-direction.x, -direction.y))
            {
                nextSpawned[i] = true;
            }
        }
    }
}
