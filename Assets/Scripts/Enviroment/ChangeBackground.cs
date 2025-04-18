using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField]
    private string currentSong; //This should be change.

    [SerializeField]
    private string nextSong;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.Instance.Stop(currentSong);
            AudioManager.Instance.Play(nextSong);
            Destroy(gameObject);
        }

    }
}
