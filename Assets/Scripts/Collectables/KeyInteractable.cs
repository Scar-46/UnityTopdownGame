using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;

public class KeyInteractable : Interactable
{
    public int keys;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            if (PlayerStats.Instance.WithdrawCoins(price))
            {
                PlayerStats.Instance.AddKeys(keys);
                Destroy(gameObject);
            }
        }
    }
}
