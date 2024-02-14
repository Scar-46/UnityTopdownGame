using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : Interactable
{
    public GameObject loot;
    private float _x, _y;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            _x = Random.Range(-1, 1); // Spawn in diferent points
            _y = Random.Range(-1, 1);

            if (PlayerStats.Instance.RemoveKeys(price))
            {
                animator.SetTrigger("Open");
                
                Vector2 postition = new Vector2(gameObject.transform.position.x + _x, gameObject.transform.position.y +_y);

                Instantiate(loot, postition, Quaternion.identity);
                if (priceBox != null)
                {
                    priceBox.SetActive(false);
                }
                Destroy(this);
            }
        }
    }
}
