using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

#nullable enable
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public GameObject player;
    public float health;
    public float maxHealth = 100;
    public Slider? CurrentHealth;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinsCounter;

    //Damage
    public CameraShake cameraShake;
    public float cameraShakeTime;
    public float cameraShakeForce;


    public int coins;


    private void Awake()
    {
        if (Instance is not null )
        {
            Destroy( Instance );
        }
        else
        {
           PlayerStats.Instance = this;
           DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;
        CurrentHealth.value = 1;
        healthText.text = Mathf.Ceil(health).ToString() + " / " + maxHealth.ToString();
    }
    public void HealCaracter(float heal)
    {
        health += heal;
        CheckOverheal();
        SetHealthUI();
    }

    private void SetHealthUI()
    {
        CurrentHealth.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(health).ToString() + " / " + maxHealth.ToString();
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
        cameraShake.Shake(cameraShakeForce, cameraShakeTime);
        health -= damage;
        CheckDeath();
        SetHealthUI();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            health = 0;
            Destroy(player);
        }
    }
    private float CalculateHealthPercentage()
    {
        return (health / maxHealth);
    }

    public void AddCurrency(int amount)
    {
        coins += amount;
        coinsCounter.text = "Coins: " + coins.ToString();
    }
}
#nullable disable
