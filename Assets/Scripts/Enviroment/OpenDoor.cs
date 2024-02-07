using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    private Animator _Animator;
    public bool needKey = true;
    public Collider2D stopper;


    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && needKey)
        {
            if (PlayerStats.Instance.RemoveKeys(1))
            {
                _Animator.SetTrigger("RemoveLock");
                needKey = false;
            }
        }
        else if (collision.name == "Player")
        {
            _Animator.SetTrigger("Open");
        }
    }
    public void UnlockDoor()
    {
        stopper.enabled = false;
    }

    public void LockDoor()
    {
        stopper.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" && !needKey)
        {
            _Animator.SetTrigger("Close");
        }
    }
}
