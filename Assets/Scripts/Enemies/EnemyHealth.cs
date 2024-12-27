using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable enable
public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public float knockbackDelay = 0;

    public TextMeshProUGUI enemyName;
    public string enemyStringName;

    public GameObject? healthbar;
    public TextMeshProUGUI healthText;
    public Slider? CurrentHealth;

    public List<GameObject>? lootDroop = null;

    private Rigidbody2D rb2D;
    private Animator animator;

    public bool start = true;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    public void HealCharacter(float heal)
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

    public void DealDamage(float damage, Vector2 knockback)
    {
        StopAllCoroutines();
        health -= damage;
        CheckDeath();
        if (CurrentHealth)
        {
            healthbar.SetActive(true);
            if (enemyName)
            {
                enemyName.text = enemyStringName;
                healthText.text = Mathf.Ceil(health).ToString() + " / " + maxHealth.ToString();
            }
            CurrentHealth.value = CalculateHealthPercentage();
            Debug.Log(CurrentHealth.value);
        }

        rb2D.AddForce(knockback, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
        
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDelay);
        rb2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(knockbackDelay);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
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
            if (lootDroop is not null)
            {
                foreach (var loot in lootDroop)
                {
                    Instantiate(loot, transform.position, Quaternion.identity);
                }
            }
        }
        else if (start)
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
