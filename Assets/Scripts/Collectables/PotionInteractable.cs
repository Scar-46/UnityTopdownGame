using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionInteractable : Interactable
{
    public GameObject potion;
    public Transform position;

    private void Awake()
    {
        position = GameObject.Find("UIOverlay").GetComponent<Transform>().Find("Slot");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            Instantiate(potion, position);
            Destroy(gameObject);
        }
    }
}
