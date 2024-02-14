using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Room 
{
    public List<Vector2Int> directions;

    public bool collapsed = false;

    public HashSet<GameObject> roomsAvailable; // All the posible rooms in this tile
    public TilemapMatrix roomTemplates;

    public  Room(TilemapMatrix matrix)
    {
        directions = new List<Vector2Int>();
        directions.AddRange(new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right });
        roomsAvailable = new HashSet<GameObject>();
        roomTemplates = matrix;
        StartRoomPrefabs();
    }
    public GameObject GetRoom(int x, int y)
    {
        int roomIndex = Random.Range(0, roomsAvailable.Count);
        GameObject[] rooms = new GameObject[roomsAvailable.Count];
        roomsAvailable.CopyTo(rooms);

        directions.Clear();
        directions.AddRange(rooms[roomIndex].GetComponent<RoomEntries>().Entries);

        Debug.Log("Rooms " + rooms.Length);
        collapsed = true;
        return rooms[roomIndex];
    }

    public void RemoveRoomTile(Vector2Int entry)
    {
        directions.Remove(new Vector2Int(-entry.x, -entry.y));
        GetRoomPrefabs();
        Debug.Log("Remove "+ -entry.x + " " + -entry.y);
    }

    public void RemoveOthers(Vector2Int entry)
    {
        for (int i = 0; i < directions.Count; i++)
        {
            if (directions[i] != entry)
            {
                directions.Remove(directions[i]);
                Debug.Log("Direction remove " + directions[i]);
            }
        }
        GetRoomPrefabs();
    }

    private void StartRoomPrefabs()
    {
        roomsAvailable.UnionWith(roomTemplates.topRooms);
        roomsAvailable.UnionWith(roomTemplates.bottomRooms);
        roomsAvailable.UnionWith(roomTemplates.leftRooms);
        roomsAvailable.UnionWith(roomTemplates.rightRooms);

        Debug.Log("Count " +  roomsAvailable.Count);
    }

    private void GetRoomPrefabs()
    {
        if (!directions.Contains(Vector2Int.up))
        {
            roomsAvailable.ExceptWith(roomTemplates.topRooms);
        } 
        else if (directions.Contains(Vector2Int.down))
        {
            roomsAvailable.ExceptWith(roomTemplates.topRooms);
        } 
        else if (directions.Contains(Vector2Int.left))
        {
            roomsAvailable.ExceptWith(roomTemplates.leftRooms);
        } 
        else if (directions.Contains(Vector2Int.right))
        {
            roomsAvailable.ExceptWith(roomTemplates.rightRooms);
        }
    }
}
