using UnityEngine;

public class IceBookItem : Item
{
    public GameObject iceSpell;

    public int magicConsumed = 0;

    private GameObject activeSpell;

    private bool _Activated = false;

    public override void UseItem()
    {
        if (!_Activated && PlayerStats.Instance.magic > 0)
        {

            activeSpell = Instantiate(iceSpell, player.transform.position, Quaternion.identity, player.transform);
            _Activated = true;
            PlayerStats.Instance.UseMagic(magicConsumed);
        }
        else
        {
            _Activated = false;
            Destroy(activeSpell);
        }
    }
}
