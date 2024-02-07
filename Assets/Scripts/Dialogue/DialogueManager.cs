using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public GameObject textBox;
    public float dialogueSpeed = 0.04f;
    public float timeBetweenDialogues = 3f;

    Message[] currentMessages;
    Actor[] currentActors;

    int activeMessages = 0;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
        }
        else
        {
            DialogueManager.Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessages = 0;

        StartCoroutine(DisplayDialogue()); //This should be fix, words can be scramble if activated several times.
    }

    public IEnumerator DisplayDialogue()
    {

        Message messageToDisplay = currentMessages[activeMessages];
        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.actorName;
        textBox.transform.position = actorToDisplay.position;
        textBox.SetActive(true);

        string fullMessage = messageToDisplay.message;
        messageText.text = "";

        for (int i = 0; i < fullMessage.Length; i++) 
        {
            messageText.text += fullMessage[i];
            yield return new WaitForSeconds(dialogueSpeed);
        }

        yield return new WaitForSeconds(timeBetweenDialogues);
        NextMessage();
    }

    public void NextMessage()
    {
        activeMessages++;

        if (currentMessages.Length > activeMessages)
        {
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            textBox.SetActive(false);
        }
    }
}
