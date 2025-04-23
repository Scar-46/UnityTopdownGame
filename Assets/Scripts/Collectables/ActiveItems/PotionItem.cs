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
        slot = GameObject.Find("UIOverlay").GetComponent<Transform>().Find("Panel").GetComponent<Transform>().Find("Slot").GetComponent<Slot>();
    }

    public override void UseItem()
    {
        PlayerStats.Instance.HealCharacter(health);
        PlayerStats.Instance.RecoverMagic(mana);
        slot.isOccupied = false;
        Destroy(gameObject);
    }
}
