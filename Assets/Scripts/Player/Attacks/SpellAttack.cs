using System.Collections;
using UnityEngine;

public class SpellAttack : PlayerAttack
{
    public GameObject projectile;
    public float projectileForce;
    public float magicConsumed = 0;

    private void Start()
    {
        knockbackForce = 16;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {

            if (PlayerStats.Instance.magic <= 0 || attackBlocked)
                return;

            AudioManager.Instance.Play("MagicAttack");
            animator.SetTrigger("MagicAttack");
            attackBlocked = true;
            StartCoroutine(DelayAttack());

            // Instantiate proyectile.
            GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);

            // Calculate mouse direction.
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = transform.position;
            Vector2 direction = (mousePos - playerPos).normalized;

            // Calculate rotation angle.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate proyectile.
            spell.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            spell.GetComponent<ProjectileDamage>().knockbackforce = knockbackForce;
            spell.GetComponent<ProjectileDamage>().damage = Random.Range(minDamage, maxDamage);

            //Consume magic
            PlayerStats.Instance.UseMagic(magicConsumed);
        }
    }
}
