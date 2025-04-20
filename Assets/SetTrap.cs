using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class SetTrap : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private float timer;

    Animator _Animator;


    private void Awake()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Step");
        _Animator.SetTrigger("Step");

        if (collider.transform.tag == "Enemy")
        {
            collider.GetComponent<EnemyHealth>().DealDamage(damage, Vector2.zero);
        }

        if (collider.transform.tag == "Player")
        {
            PlayerStats.Instance.DealDamage(damage);
        }
    }
}
