using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

#nullable enable
public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public float knockbackDelay = 0;
    public bool isObject = false;

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

            AudioManager.Instance.Play(isObject ? "Attack" : "EnemyDeath");

            // Drop loot if available
            if (lootDroop != null)
            {
                foreach (var loot in lootDroop)
                {
                    Instantiate(loot, transform.position, Quaternion.identity);
                }
            }

            // Destroy all non-essential components
            var components = gameObject.GetComponents<Component>();
            foreach (var comp in components)
            {
                if (!(comp is Transform || comp is Animator || comp is SpriteRenderer))
                {
                    Destroy(comp);
                }
            }

            // Destroy all child objects to reduce overhead
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        else if (start)
        {
            AudioManager.Instance.Stop("Miss");
            AudioManager.Instance.Play("Attack");
            animator.SetTrigger("Damage");
        }
    }

    private float CalculateHealthPercentage()
    {
        return (health / maxHealth);
    }
}
#nullable disable
