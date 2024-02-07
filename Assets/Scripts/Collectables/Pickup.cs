using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public enum PickupObject {COIN, KEY};
    public PickupObject currentObject;
    public int pickupValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (currentObject == PickupObject.COIN)
            {
                PlayerStats.Instance.AddCoins(pickupValue);

            }
            else if (currentObject == PickupObject.KEY)
            {
                PlayerStats.Instance.AddKeys(pickupValue);
            }
            Destroy(gameObject);
        }
    }
}
