using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Item : MonoBehaviour
{
    public GameObject pickable;
    protected GameObject player;

    public bool drop = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseItem();
        }
    }

    public abstract void UseItem();

    public void OnDestroy()
    {
        if (drop)
        {
            Instantiate(pickable, player.transform.position, Quaternion.identity);
        }
    }
}
