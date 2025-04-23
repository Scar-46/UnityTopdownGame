using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StartRoom : MonoBehaviour
{
    private Light2D[] lights;
    private GameObject enviroment;
    public GameObject[] spawners;
    public GameObject startPoints;

    private bool start = true;

    public static event Action? OnRoomStarted;
    public bool enemyRoom = true;

    private void Awake()
    {
        if (transform.parent != null && transform.parent.parent != null)
        {
            enviroment = transform.parent.parent.Find("Enviroment")?.gameObject;
            startPoints = transform.parent.gameObject;

            if (enviroment != null)
            {
                lights = enviroment.GetComponentsInChildren<Light2D>();
            }
            else
            {
                Debug.Log("Enviroment GameObject not found!");
            }
        }
        else
        {
            Debug.Log("Parent or parent's parent is missing!");
        }
    }
    public void ActivateSpawns()
    {
        foreach (var spawn in spawners)
        {
            spawn.SetActive(true);
        }
    }

    public void ActivateLights()
    {
        foreach (var light in lights)
        {
            light.enabled = true;
            Animator animator = light.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Ignite");
                light.GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && start)
        {
            if (enemyRoom)
            {
                OnRoomStarted.Invoke();
            }

            ActivateLights();
            ActivateSpawns();
            start = false;
            foreach (var child in startPoints.GetComponentsInChildren<Transform>())
            {
                var childStartRoom = child.GetComponent<StartRoom>();
                Destroy(childStartRoom);
            }

        }
    }
}
