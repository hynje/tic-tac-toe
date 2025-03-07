using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingPanelController : MonoBehaviour
{
    [SerializeField] TMP_InputField messageInputField;
    [SerializeField] GameObject messageTextPrefab;
    [SerializeField] Transform messageTextParent;

    private MultiplayManager _multiplayManager;
    private string _roomId;
    
    public void OnEndEditInputField(string messageText)
    {
        var messageTextObject = Instantiate(messageTextPrefab, messageTextParent);
        messageTextObject.GetComponent<TMP_Text>().text = messageText;
        messageInputField.text = "";

        if (_roomId != null && _multiplayManager != null)
        {
            _multiplayManager.SendMessage(_roomId, "nickName", messageText);
        }
    }

    private void Start()
    {
        messageInputField.interactable = false;
        _multiplayManager = new MultiplayManager((state, id) =>
        {
            switch (state)
            {
                case Constants.MultiplayManagerState.CreateRoom:
                    Debug.Log("## Create Room");
                    _roomId = id;
                    break;
                case Constants.MultiplayManagerState.JoinRoom:
                    Debug.Log("## Join Room");
                    _roomId = id;
                    messageInputField.interactable = true;
                    break;
                case Constants.MultiplayManagerState.StartGame:
                    Debug.Log("## Start Game");
                    messageInputField.interactable = true;
                    break;
                case Constants.MultiplayManagerState.EndGame:
                    Debug.Log("## End Game");
                    break;
            }
        });
        _multiplayManager.OnReceiveMessage =  OnReceiveMessage;
    }

    private void OnReceiveMessage(MessageData messageData)
    {
        UnityThread.executeInUpdate(() =>
        {
            var  messageText = Instantiate(messageTextPrefab, messageTextParent);
            messageText.GetComponent<TextMeshProUGUI>().text = messageData.nickName + " : " + messageData.message;
        });
    }
}
