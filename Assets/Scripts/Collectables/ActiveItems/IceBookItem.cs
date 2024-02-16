using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBookItem : Item
{
    public GameObject iceSpell;


    private GameObject activeSpell;

    private bool _Activated = false;

    public override void UseItem()
    {
        if (!_Activated)
        {
            activeSpell = Instantiate(iceSpell, player.transform.position, Quaternion.identity, player.transform);
            _Activated = true;
        }
        else
        {
            _Activated = false;
            Destroy(activeSpell);
        }
    }
}
