using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public enum PickupObject {COIN, GEM};
    public PickupObject currentObject;
    public int pickupValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (currentObject == PickupObject.COIN)
            {
                print("Collected");
                PlayerStats.Instance.AddCurrency(pickupValue);

            }
            else if (currentObject == PickupObject.GEM)
            {

            }
            Destroy(gameObject);
        }
    }
}
