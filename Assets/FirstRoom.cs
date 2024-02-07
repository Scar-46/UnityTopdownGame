using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirstRoom : MonoBehaviour
{
    // Start is called before the first frame update

    public Light2D[] lights;

    void Start()
    {
        Debug.Log("Hello");
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
}
