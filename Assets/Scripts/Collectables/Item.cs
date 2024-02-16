using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int health;
    public int mana;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerStats.Instance.HealCaracter(health);
            PlayerStats.Instance.recoverMagic(mana);
            Destroy(gameObject);
        }
    }
}
