using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable enable
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    public GameObject player;

    //Health
    private float health;
    private float maxHealth = 100;
    public Slider? CurrentHealth;
    public TextMeshProUGUI healthText;

    //Magic
    public float magic; // TODO: make this private.
    private float maxMagic = 100;
    public Slider? CurrentMagic;
    public TextMeshProUGUI magicText;


    //Damage
    public CameraShake cameraShake;
    public float cameraShakeTime;
    public float cameraShakeForce;
    public Animator playerAnimator;


    private int coins;
    public TextMeshProUGUI coinsCounter;

    private int keys;
    public TextMeshProUGUI keysCounter;


    private void Awake()
    {
        if (Instance is not null )
        {
            Destroy(gameObject);
        }
        else
        {
           PlayerStats.Instance = this;
           DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        //Set Player
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();

        //Set health
        health = maxHealth;
        CurrentHealth.value = 1;
        healthText.text = Mathf.Ceil(health).ToString() + " / " + maxHealth.ToString();

        //Set magic
        magic = maxMagic;
        CurrentMagic.value = 1;
        magicText.text = Mathf.Ceil(health).ToString() + " / " + maxMagic.ToString();

        DontDestroyOnLoad(gameObject);
    }


    //------------Health------------//
    public void HealCaracter(float heal)
    {
        health += heal;
        CheckOverheal();
        SetHealthUI();
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
            playerAnimator.SetTrigger("Death");
            foreach (var comp in player.GetComponents<Component>())
            {
                if (!(comp is Transform || comp is Animator || comp is SpriteRenderer))
                {
                    Destroy(comp);
                }
            }
            //Destroy(player);
        }
        else
        {
            playerAnimator.SetTrigger("Damage");
        }
    }

    private void SetHealthUI()
    {
        CurrentHealth.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(health).ToString() + " / " + maxHealth.ToString();
    }
    private float CalculateHealthPercentage()
    {
        return (health / maxHealth);
    }

    //------------Magic------------//
    public void UseMagic(float magicUsed)
    {
        magic -= magicUsed;
        SetMagicUI();
    }

    public void recoverMagic(float amount)
    {
        magic += amount;
        CheckOvermagic();
        SetMagicUI();
    }

    private void CheckOvermagic()
    {
        if (magic > maxMagic)
        {
            magic = maxMagic;
        }
    }

    private void SetMagicUI()
    {
        CurrentMagic.value = CalculateMagicPercentage();
        magicText.text = Mathf.Ceil(magic).ToString() + " / " + maxMagic.ToString();
    }

    private float CalculateMagicPercentage()
    {
        return (magic / maxMagic);
    }

    //------------Currency------------//
    public void AddCoins(int amount)
    {
        coins += amount;
        coinsCounter.text = "Coins: " + coins.ToString();
    }

    public bool WithdrawCoins(int amount)
    {
        if (coins >= amount) { 
            coins -= amount;
            coinsCounter.text = "Coins: " + coins.ToString();
            return true;
        }
        else { return false; }
    }

    public void AddKeys(int amount)
    {
        keys += amount;
        keysCounter.text = "Keys: " + keys.ToString();
    }

    public bool RemoveKeys(int amount)
    {
        if (keys >= amount)
        {
            keys -= amount;
            keysCounter.text = "Keys: " + keys.ToString();
            return true;
        }
        else { return false;}

    }

 }
#nullable disable
