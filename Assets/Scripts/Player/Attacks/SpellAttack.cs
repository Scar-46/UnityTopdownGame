using System.Collections;
using UnityEngine;

public class SpellAttack : PlayerAttack
{
    public GameObject projectilePrefab;
    public float projectileForce;
    public float magicConsumed = 0;
    public float spawnDelay = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CastSpell();
        }
    }

    private void CastSpell()
    {
        if (attackBlocked) return;
        if (PlayerStats.Instance.magic < magicConsumed) return;

        attackBlocked = true;

        // Play animation and sound
        animator.SetTrigger("MagicAttack");
        AudioManager.Instance.Play("MagicAttack");

        // Instantiate projectile
        StartCoroutine(SpawnSpellWithDelay(spawnDelay));

        // Consume magic
        PlayerStats.Instance.UseMagic(magicConsumed);

        // Unlock attack after delay
        StartCoroutine(DelayAttack());
    }

    private IEnumerator SpawnSpellWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        // Instantiate projectile at player position
        GameObject spell = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Aim towards mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 direction = (mousePos - playerPos).normalized;

        // Rotate projectile
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spell.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Apply velocity
        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileForce;

        // Set projectile damage & knockback
        ProjectileDamage proj = spell.GetComponent<ProjectileDamage>();
        proj.knockbackforce = knockbackForce;
        proj.damage = GetRandomDamage();
    }
}
