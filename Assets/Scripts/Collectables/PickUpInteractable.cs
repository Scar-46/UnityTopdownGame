using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpInteractable : Interactable
{
    public GameObject slotItem;
    public Slot slot;

    private void Awake()
    {
        slot = GameObject.Find("UIOverlay").GetComponent<Transform>().Find("Slot").GetComponent<Slot>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            if (PlayerStats.Instance.WithdrawCoins(price))
            {
                slot.setItem(slotItem);
                Destroy(gameObject);
            }
        }
    }
}
