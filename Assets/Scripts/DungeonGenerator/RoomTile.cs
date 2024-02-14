using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTile : ScriptableObject
{
    public bool collapsed = false;
    public int entropy; //How many different rooms are available
    public GameObject[] roomsAvailable; // All the posible rooms in this tile


    public void updateRoomTile()
    {

    }
}
