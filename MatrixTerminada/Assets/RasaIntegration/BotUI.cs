using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the gameobjects and methods for interacting with the UI.
/// </summary>
public class BotUI : MonoBehaviour {
    public GameObject       UICanvas;
    public GameObject       contentDisplayObject;               // Text gameobject where all the conversation is shown
    public InputField       input;                              // InputField gameobject wher user types their message

    public GameObject       userBubble;                         // reference to user chat bubble prefab
    public GameObject       botBubble;                          // reference to bot chat bubble prefab

    private const int       messagePadding = 15;                // space between chat bubbles 
    private int             allMessagesHeight = messagePadding; // int to keep track of where next message should be rendered
    public bool             increaseContentObjectHeight;        // bool to check if content object height should be increased

    //public ChatManager   chatManager;                           // reference to ChatManager script

    /// This method is used to update the display panel with the user's and bot's messages.
    /// The one who wrote this message
    /// "message" The message
    public void UpdateDisplay (string sender, string message, string messageType) {
        // Create chat bubble and add components
        GameObject chatBubbleChild = CreateChatBubble(sender);

        AddChatComponent(chatBubbleChild, message, messageType);

        // Set chat bubble position
        StartCoroutine(SetChatBubblePosition(chatBubbleChild.transform.parent.GetComponent<RectTransform>(), sender));
    }

    /// Coroutine to set the position of the chat bubble inside the contentDisplayObject.
    /// "chatBubblePos" RectTransform of chat bubble
    /// "sender" Sender who sent the message
    private IEnumerator SetChatBubblePosition (RectTransform chatBubblePos, string sender) {
        // Wait for end of frame before calculating UI transform
        yield return new WaitForEndOfFrame();

        // get horizontal position based on sender
        int horizontalPos = 0;
        if (sender == "user") {
            horizontalPos = 330;
        } else if (sender == "bot") {
            horizontalPos = 20;
        }

        // set the vertical position of chat bubble
        allMessagesHeight += 15 + (int)chatBubblePos.sizeDelta.y;
        chatBubblePos.anchoredPosition3D = new Vector3(horizontalPos, -allMessagesHeight, 0);

        if (allMessagesHeight > 340) {
            // update contentDisplayObject hieght
            RectTransform contentRect = contentDisplayObject.GetComponent<RectTransform>();
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, allMessagesHeight + messagePadding);
            contentDisplayObject.transform.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0;
        }
    }

    /// Coroutine to update chat bubble positions based on their size.
    public IEnumerator RefreshChatBubblePosition () {
        // Wait for end of frame before calculating UI transform
        yield return new WaitForEndOfFrame();

        // refresh position of all gameobjects based on size
        int localAllMessagesHeight = messagePadding;
        foreach (RectTransform chatBubbleRect in contentDisplayObject.GetComponent<RectTransform>()) {
            if (chatBubbleRect.sizeDelta.y < 35) {
                localAllMessagesHeight += 35 + messagePadding;
            } else {
                localAllMessagesHeight += (int)chatBubbleRect.sizeDelta.y + messagePadding;
            }
            chatBubbleRect.anchoredPosition3D =
                    new Vector3(chatBubbleRect.anchoredPosition3D.x, -localAllMessagesHeight, 0);
        }

        // Update global message Height variable
        allMessagesHeight = localAllMessagesHeight;
        if (allMessagesHeight > 340) {
            // update contentDisplayObject hieght
            RectTransform contentRect = contentDisplayObject.GetComponent<RectTransform>();
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, allMessagesHeight + messagePadding);
            contentDisplayObject.transform.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0;
        }
    }

    /// This method creates chat bubbles from prefabs and sets their positions.
    /// "sender" The sender of message for which bubble is rendered
    /// Reference to empty gameobject on which message components can be added
    private GameObject CreateChatBubble (string sender) {
        GameObject chat = null;
        if (sender == "user") {
            // Create user chat bubble from prefabs and set it's position
            chat = Instantiate(userBubble);
            chat.transform.SetParent(contentDisplayObject.transform, false);
        } else if (sender == "bot") {
            // Create bot chat bubble from prefabs and set it's position
            chat = Instantiate(botBubble);
            chat.transform.SetParent(contentDisplayObject.transform, false);
        }

        // Add content size fitter
        ContentSizeFitter chatSize = chat.AddComponent<ContentSizeFitter>();
        chatSize.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        chatSize.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Add vertical layout group
        VerticalLayoutGroup verticalLayout = chat.AddComponent<VerticalLayoutGroup>();
        if (sender == "user") {
            verticalLayout.padding = new RectOffset(10, 20, 40, 40);
        } else if (sender == "bot") {
            verticalLayout.padding = new RectOffset(20, 10, 40, 40);
        }
        verticalLayout.childAlignment = TextAnchor.MiddleCenter;

        // Return empty gameobject on which chat components will be added
        return chat.transform.GetChild(0).gameObject;
    }

    /// This method adds message component to chat bubbles based on message type.
    /// "chatBubbleObject" The empty gameobject under chat bubble
    /// "message" message to be shown
    /// "messageType" The type of message (text, image etc)
    private void AddChatComponent (GameObject chatBubbleObject, string message, string messageType) {
        switch (messageType) {
            case "text":
                // Create and init Text component
                Text chatMessage = chatBubbleObject.AddComponent<Text>();

                // add font as it is none at times when creating text component from script
                chatMessage.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                chatMessage.fontSize = 18;
                chatMessage.text = message;
                chatMessage.alignment = TextAnchor.MiddleLeft;
                chatMessage.horizontalOverflow = HorizontalWrapMode.Wrap;

                break;
            case "image":
                // Create and init Image component
                //Image chatImage = chatBubbleObject.AddComponent<Image>();
                //StartCoroutine(chatManager.SetImageTextureFromUrl(message, chatImage));
                break;
            case "attachment":
                break;
            case "buttons":
                break;
            case "elements":
                break;
            case "quick_replies":
                break;
        }
    }
}
