using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilemapMatrix : MonoBehaviour
{

    public Room[,] tiles; // Store the actual state of every tile
    public Vector2 dim = new Vector2(10,10);
    public int maxRooms;
    public int roomCounter = 0;

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    void Start()
    {
        // Initialize the tilemap array
        tiles = new Room[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tiles[i, j] = new Room(this);
            }
        }

        // Populate the matrix with tilemap prefabs
        int firstX = Random.Range(0, (int)dim.x);
        int firstY = Random.Range(0, (int)dim.y);

        Debug.Log(firstX + " " + firstY);
        PopulateMatrix(firstX, firstY);
    }

    void PopulateMatrix(int x, int y)
    {
        GameObject room = tiles[x, y].GetRoom(x, y);

        Instantiate(room, new Vector2(x * 20, y * 12), Quaternion.identity);
        roomCounter++;
        foreach (var direction in tiles[x,y].directions)
        {
            Debug.Log("Directions:");
            Debug.Log("D " + direction);
        }
        UpdateRooms(x, y, tiles[x, y]);
    }

    private void UpdateRooms(int previousX, int previousY, Room room)
    {
        // Iterate over each direction
        foreach (Vector2Int direction in directions)
        {
            int adjacentX = previousX + direction.x;
            int adjacentY = previousY + direction.y;

            bool matchingConnection = false;

            foreach (Vector2Int roomDi in room.directions)
            {
                if (roomDi == direction)
                {
                    matchingConnection = true;
                }
            }
            if (!matchingConnection && InRange(adjacentX, adjacentY))
            {
                
                Debug.Log("No match " + adjacentX + " " + adjacentY);
                tiles[adjacentX, adjacentY].RemoveRoomTile(direction);
            } 
            else if (InRange(adjacentX, adjacentY)) 
            {
                Debug.Log("Match " + adjacentX + " " + adjacentY);
                tiles[adjacentX, adjacentY].RemoveOthers(direction);
            }
        }
        GetNextTile();
    }

    private bool InRange(int x, int y)
    {
        if ((x < dim.x && x >= 0) && (y < dim.y && y >= 0))
        {
            return true;
        }
        return false;
    }
    private void GetNextTile()
    {
        int nextX = 0;
        int nextY = 0;
        int entropy = 1000;

        for (int x = 0; x < dim.x; x++)
        {
            for (int y = 0; y < dim.y; y++)
            {
                if (tiles[x, y].collapsed == false &&  entropy > tiles[x, y].roomsAvailable.Count)
                {
                    nextX = x;
                    nextY = y;
                    entropy = tiles[x, y].roomsAvailable.Count;
                }
            }
        }
        Debug.Log("NextTile " + tiles[nextX, nextY].roomsAvailable.Count);
        Debug.Log("Pos " + nextX + ", " + nextY);

        if (roomCounter <= maxRooms)
        {
            PopulateMatrix(nextX, nextY);
        }
    }
}
