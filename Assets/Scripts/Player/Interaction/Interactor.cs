using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    [SerializeField]
    private float radius = 1.3f;

    [SerializeField]
    private LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = Physics2D.OverlapCircleAll(this.transform.position, radius, layerMask).FirstOrDefault();
        if (collider != null)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
        }
    }
}
