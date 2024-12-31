using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    private Animator _Animator;
    public bool needKey = true;
    public Collider2D stopper;
    public GameObject LowerDoor;


    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && needKey)
        {
            if (PlayerStats.Instance.RemoveKeys(1))
            {
                _Animator.SetTrigger("RemoveLock");
                needKey = false;
            }
        }
        else if (collision.tag == "Player")
        {
            _Animator.SetTrigger("Open");
        }
    }
    public void UnlockDoor()
    {
        stopper.enabled = false;

        if (LowerDoor != null)
        {
            LowerDoor.SetActive(true);
        }
    }

    public void LockDoor()
    {
        stopper.enabled = true;

        if (LowerDoor != null)
        {
            LowerDoor.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !needKey)
        {
            _Animator.SetTrigger("Close");
        }
    }
}
