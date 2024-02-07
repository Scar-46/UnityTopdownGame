using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool firstEntry = true;


    private void Start()
    {
        actors[0].position = gameObject.transform.position;
        actors[0].position.y = actors[0].position.y + 0.82f;
    }
    public void StartDialogue()
    {
        DialogueManager.Instance.OpenDialogue(messages, actors);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Player" && firstEntry)
        {
            firstEntry = false;
            StartDialogue();
        }
    }
}

[System.Serializable]
public class Message
{
    public int actorId;

    [TextArea(0,100)]
    public string message;
}

[System.Serializable]
public class Actor
{
    public string actorName;
    public Vector3 position;
}