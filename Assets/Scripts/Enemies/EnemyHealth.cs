using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

#nullable enable
public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;

    public GameObject? healthbar;
    public Slider? CurrentHealth;

    public GameObject? lootDroop;
    public GameObject? deathVariant;

    private Rigidbody2D rb2D;
    private Animator animator;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        if (healthbar)
        {
            healthbar.SetActive(true);
        }
    }

    public void HealCaracter(float heal)
    {
        health += heal;
        CheckOverheal();
        if (CurrentHealth)
        {
            CurrentHealth.value = CalculateHealthPercentage();
        }

    }

    private void CheckOverheal()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        CheckDeath();
        if (CurrentHealth)
        {
            CurrentHealth.value = CalculateHealthPercentage();
        }
    }

    public void DealDamage(float damage, Vector2 knockback)
    {
        health -= damage;
        CheckDeath();
        if (CurrentHealth)
        {
            CurrentHealth.value = CalculateHealthPercentage();
        }

        rb2D.AddForce(knockback, ForceMode2D.Impulse);
        
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            foreach (var comp in gameObject.GetComponents<Component>())
            {
                if (!(comp is Transform || comp is Animator || comp is SpriteRenderer))
                {
                    Destroy(comp);
                }
            }
            Instantiate(lootDroop, transform.position, Quaternion.identity);
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    private float CalculateHealthPercentage()
    {
        return (health / maxHealth);
    }
}
#nullable disable
