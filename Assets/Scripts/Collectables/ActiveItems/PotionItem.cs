using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : Item
{
    public int health;
    public int mana;
    public Slot slot;

    private void Awake()
    {
        slot = GameObject.Find("UIOverlay").GetComponent<Transform>().Find("Slot").GetComponent<Slot>();
    }

    public override void UseItem()
    {
        PlayerStats.Instance.HealCaracter(health);
        PlayerStats.Instance.recoverMagic(mana);
        slot.isOccupied = false;
        Destroy(gameObject);
    }
}
