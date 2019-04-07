using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicChat : MonoBehaviour
{

    private bool windowUp = false;
    [SerializeField] private Image ArrowImage;
    [SerializeField] private Sprite UpArrowSprite;
    [SerializeField] private Sprite BottomArrowSprite;
    [SerializeField] private GameObject chatPanel, textObject, input;

    public List<Message> messageList = new List<Message>();
    public static bool newMessage = false;
    public static string newMessageText;
    public void sendPublicMsg()
    {
        Communicator.sendMessage("00005#" + input.GetComponent<Text>().text);
    }

    public void displayPublicMsg(string msg)
    {
        if (messageList.Count >= 50)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMsg = new Message();
        newMsg.text = msg;
        GameObject newTextObj = Instantiate(textObject, chatPanel.transform);
        newMsg.textObject = newTextObj.GetComponent<Text>();
        newMsg.textObject.text = newMsg.text;
        messageList.Add(newMsg);
    }

    public void ChangeOnlineFirst()
    {
        windowUp = !windowUp;
        RectTransform fui = transform.GetComponent<RectTransform>();
        if (windowUp)
        {

            ArrowImage.sprite = BottomArrowSprite;
            fui.anchoredPosition = new Vector2(0f, 330f);

        }
        else
        {
            
            ArrowImage.sprite = UpArrowSprite;
            fui.anchoredPosition = new Vector2(0f, 0f);

        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (newMessage)
        {
            newMessage = false;
            Debug.Log(newMessageText);
            displayPublicMsg(newMessageText);
        }
    }
}

public class Message
{
    public string text;
    public Text textObject;
}