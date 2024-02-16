using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        GetBossHealth getBossHealth = GameObject.Find("UIOverlay").GetComponent<GetBossHealth>();

        enemyHealth.healthbar = getBossHealth.healthbar;

        enemyHealth.healthText = getBossHealth.healthText;
        enemyHealth.enemyName = getBossHealth.enemyName;
        enemyHealth.CurrentHealth = getBossHealth.CurrentHealth;
    }
}
