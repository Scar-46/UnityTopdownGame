using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    protected bool isInRange = false;
    public Animator animator;

    //Store
    public bool isStore = false;
    public bool needKey = false;
    public int price;
    public GameObject priceBox;
    public TextMeshProUGUI priceText;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (isStore && priceText)
        {
            priceText.text = price.ToString() + "c";
        }
        if (needKey && priceText)
        {
            priceText.text = price.ToString() + "k";
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (priceBox)
            {
                priceBox.SetActive(true);
            }
            animator.SetTrigger("Shine");
            isInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (priceBox)
        {
            priceBox.SetActive(false);
        }
        animator.SetTrigger("StopShine");
        isInRange = false;
    }
}
