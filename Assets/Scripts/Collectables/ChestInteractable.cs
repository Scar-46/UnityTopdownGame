using UnityEngine;

public class ChestInteractable : Interactable
{
    public SpawnableObject[] spawnableObjects;
    private float _x, _y;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            _x = Random.Range(-1f, 1f);
            _y = Random.Range(-1f, 1f);

            if (PlayerStats.Instance.RemoveKeys(price))
            {
                animator.SetTrigger("Open");

                Vector2 position = new Vector2(transform.position.x + _x, transform.position.y + _y);
                LootSpawner.SpawnLoot(spawnableObjects, position);

                if (priceBox != null)
                {
                    priceBox.SetActive(false);
                }
                Destroy(this);
            }
        }
    }
}
