using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpellAttack : MonoBehaviour
{
    public GameObject projectile;
    public float minDamage;
    public float maxDamage;
    public float projectileForce;
    public float knockbackForce = 100f;
    public float magicConsumed = 0;

    public Animator animator;
    public bool attackBloked = false;
    public float attackDelay = 0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {

            if (PlayerStats.Instance.magic <= 0 || attackBloked)
                return;

            AudioManager.Instance.Play("MagicAttack");
            animator.SetTrigger("MagicAttack");
            attackBloked = true;
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
            spell.GetComponent<ProjectileDamage>().damage = Random.Range(minDamage, maxDamage);

            //Consume magic
            PlayerStats.Instance.UseMagic(magicConsumed);
        }
    }
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        attackBloked = false;
    }
}
