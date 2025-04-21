using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrap : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;

    [SerializeField]
    private float timer = 0.25f;

    private Animator _Animator;
    private HashSet<Collider2D> objectsInside = new HashSet<Collider2D>();
    private bool isCountingDown = false;

    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Track object
        if ((collider.CompareTag("Enemy") || collider.CompareTag("Player")) && !objectsInside.Contains(collider))
        {
            objectsInside.Add(collider);
        }

        // Start countdown only once
        if (!isCountingDown)
        {
            StartCoroutine(TrapCountdown());
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Remove object when it leaves
        if (objectsInside.Contains(collider))
        {
            objectsInside.Remove(collider);
        }
    }

    private IEnumerator TrapCountdown()
    {
        isCountingDown = true;

        yield return new WaitForSeconds(timer);

        _Animator.SetTrigger("Step");

        foreach (var collider in objectsInside)
        {
            if (collider != null)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<EnemyHealth>().DealDamage(damage, Vector2.zero);
                }

                if (collider.CompareTag("Player"))
                {
                    PlayerStats.Instance.DealDamage(damage);
                }
            }
        }

        // Reset trap
        objectsInside.Clear();
        isCountingDown = false;
    }
}
