using System.Collections;
using UnityEngine;

public class DarkBookItem : Item
{
    public GameObject DarkSpell;

    public float damage;
    public float magicConsumed = 0;
    public bool attackBloked = false;
    public float attackDelay = 0f;

    private int projectileNumber = 4;
    public float projectileForce;
    public float projectileRotation;
    public float spinSpeed = 50f;


    public float angleChangeAmount = 2;
    private float intialAngle = 0;

    public override void UseItem()
    {
        if (PlayerStats.Instance.magic <= 0 || attackBloked)
            return;

        attackBloked = true;
        StartCoroutine(DelayAttack());
        for (int i = 0; i < projectileNumber; i++)
        {
            Debug.Log("Hello");
            float angle = intialAngle + i * projectileRotation;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            var spell = Instantiate(DarkSpell, player.transform.position, rotation);

            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            float angle2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            spell.GetComponent<ProjectileDamage>().damage = damage;
        }
        //Consume magic
        PlayerStats.Instance.UseMagic(magicConsumed);
        intialAngle += angleChangeAmount;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBloked = false;
    }
}
