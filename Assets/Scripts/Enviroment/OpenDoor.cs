using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    private Animator _Animator;
    public bool needKey = true;
    public Collider2D stopper;
    public GameObject LowerDoor;

    [SerializeField]
    private bool roomIsClean = true;
    private bool startRoom = true;

    private void Start()
    {
        _Animator = GetComponent<Animator>();
        EnemyRoomSpawner.OnRoomClean += OnRoomClean;
        StartRoom.OnRoomStarted += OnRoomStarted;
    }
    private void OnDestroy()
    {
        EnemyRoomSpawner.OnRoomClean -= OnRoomClean;
        StartRoom.OnRoomStarted -= OnRoomStarted;
    }

    private void OnRoomStarted()
    {
        roomIsClean = false;
    }

    private void OnRoomClean()
    {
        roomIsClean = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!roomIsClean) return; // Don't open unless room is clean

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!roomIsClean && !startRoom) return;

        if (collision.tag == "Player" && !needKey)
        {
            startRoom = false;
            _Animator.SetTrigger("Close");
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
}
