using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionInteractable : Interactable
{

    public int health;
    public int mana;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {

            if (PlayerStats.Instance.WithdrawCoins(price))
            {
                PlayerStats.Instance.HealCaracter(health);
                PlayerStats.Instance.recoverMagic(mana);
                Destroy(gameObject);
            }
        }
    }
}
