using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : Interactable
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            if (PlayerStats.Instance.RemoveKeys(price))
            {
                animator.SetTrigger("Open");

                Debug.Log("Open");
                priceBox.SetActive(false);
                Destroy(this);
            }
        }
    }
}
