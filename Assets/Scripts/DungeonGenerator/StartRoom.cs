using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StartRoom : MonoBehaviour
{
    public Light2D[] lights;
    private GameObject enviroment;
    public GameObject[] spawners;

    private bool start = true;

    private void Awake()
    {
        if (transform.parent != null && transform.parent.parent != null)
        {
            enviroment = transform.parent.parent.Find("Enviroment")?.gameObject;

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
            ActivateLights();
            ActivateSpawns();
            start = false;
        }
    }
}
