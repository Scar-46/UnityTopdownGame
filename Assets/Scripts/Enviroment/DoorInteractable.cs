using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{

    public int keys;
    public int health;

    public string GetDescription()
    {
        return "Use 1 Key";
    }

    public string Interact()
    {
        if (PlayerStats.Instance.RemoveKeys(keys))
        {
           OpenDoor openDoor = gameObject.GetComponent<OpenDoor>();
            if (openDoor != null)
            {
                openDoor.needKey = false;
            }
            return "The door has been opened";
        }
        else
        {
            return "You Dont have enough keys.";
        }

    }
}
