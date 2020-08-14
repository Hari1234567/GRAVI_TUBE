using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessagePanel : MonoBehaviour
{
    private TextMeshProUGUI messageText;

    private void Start()
    {
        messageText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void printMessage(string message)
    {
        messageText.text = message;
        GetComponent<Animator>().SetTrigger("MessagePop");
        GetComponent<AudioSource>().Play();
    }
}
