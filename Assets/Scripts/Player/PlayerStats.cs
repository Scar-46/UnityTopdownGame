using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable enable
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    private GameObject? player;

    // Health
    private float health;
    private float maxHealth = 100f;
    public Slider? CurrentHealth;
    public TextMeshProUGUI? healthText;

    // Magic
    public float magic;
    private float maxMagic = 100f;
    public Slider? CurrentMagic;
    public TextMeshProUGUI? magicText;

    // Damage
    public CameraShake? cameraShake;
    public float cameraShakeTime = 0.1f;
    public float cameraShakeForce = 1.0f;
    private Animator? playerAnimator;

    // Death
    public static event Action? OnPlayerDeath;

    // Currency
    private int coins;
    public TextMeshProUGUI? coinsCounter;

    private int keys;
    public TextMeshProUGUI? keysCounter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitializeUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            InitializePlayer();
        }

        if (GameObject.FindGameObjectWithTag("VCamera") != null)
        {
            cameraShake = GameObject.FindGameObjectWithTag("VCamera").GetComponent<CameraShake>();
        }
    }

    public void InitializePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
            Debug.Log("Player successfully initialized.");
        }
    }

    private void InitializeUI()
    {
        InitializeHealthUI();
        InitializeMagicUI();
        UpdateCurrencyUI();
    }

    private void InitializeHealthUI()
    {
        health = maxHealth;
        if (CurrentHealth != null)
            CurrentHealth.value = 1f;
        if (healthText != null)
            healthText.text = $"{Mathf.Ceil(health)} / {maxHealth}";
    }

    private void InitializeMagicUI()
    {
        magic = maxMagic;
        if (CurrentMagic != null)
            CurrentMagic.value = 1f;
        if (magicText != null)
            magicText.text = $"{Mathf.Ceil(magic)} / {maxMagic}";
    }

    private void UpdateCurrencyUI()
    {
        if (coinsCounter != null)
            coinsCounter.text = coins.ToString();
        if (keysCounter != null)
            keysCounter.text = keys.ToString();
    }

    // Health
    public void HealCharacter(float heal)
    {
        health += heal;
        health = Mathf.Min(health, maxHealth);
        SetHealthUI();
    }

    public void DealDamage(float damage)
    {
        if (cameraShake != null)
            cameraShake.Shake(cameraShakeForce, cameraShakeTime);

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            HandlePlayerDeath();
        }
        else
        {
            playerAnimator?.SetTrigger("Damage");
        }
        SetHealthUI();
    }

    private void HandlePlayerDeath()
    {
        playerAnimator?.SetTrigger("Death");
        if (player != null)
        {
            foreach (var comp in player.GetComponents<Component>())
            {
                if (!(comp is Transform || comp is Animator || comp is SpriteRenderer))
                    Destroy(comp);
            }
            foreach (var comp in player.GetComponentsInChildren<Component>())
            {
                if (comp is SpellAttack || comp is MeleeAttack)
                    Destroy(comp);
            }
        }
        StartCoroutine(DelayedDeath(0.5f));
    }

    private IEnumerator DelayedDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnPlayerDeath?.Invoke();
    }

    private void SetHealthUI()
    {
        if (CurrentHealth != null)
            CurrentHealth.value = health / maxHealth;
        if (healthText != null)
            healthText.text = $"{Mathf.Ceil(health)} / {maxHealth}";
    }

    // Magic
    public void UseMagic(float magicUsed)
    {
        magic -= magicUsed;
        magic = Mathf.Max(magic, 0);
        SetMagicUI();
    }

    public void RecoverMagic(float amount)
    {
        magic += amount;
        magic = Mathf.Min(magic, maxMagic);
        SetMagicUI();
    }

    private void SetMagicUI()
    {
        if (CurrentMagic != null)
            CurrentMagic.value = magic / maxMagic;
        if (magicText != null)
            magicText.text = $"{Mathf.Ceil(magic)} / {maxMagic}";
    }

    // Currency
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCurrencyUI();
    }

    public bool WithdrawCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }

    public void AddKeys(int amount)
    {
        keys += amount;
        UpdateCurrencyUI();
    }

    public bool RemoveKeys(int amount)
    {
        if (keys >= amount)
        {
            keys -= amount;
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }

    public void ResetGameState()
    {
        // Reset halth
        health = maxHealth;
        SetHealthUI();

        // Reset magic
        magic = maxMagic;
        SetMagicUI();

        // Reset currency
        coins = 0;
        keys = 0;
        UpdateCurrencyUI();
    }
}
#nullable disable
