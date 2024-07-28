using StreamChat.Core;
using StreamChat.Core.StatefulModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
public class ChatManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private TMP_InputField messageInputField;
    private IStreamChatClient _chatClient;
    //string channelName = "2";//5 là public ,N
    string channelName = "110";//5 là public ,H


    private async void Start()
    {
        _chatClient = StreamChatClient.CreateDefaultClient();
        await ConnectAsync();  // Tự động kết nối khi khởi động ứng dụng
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("av");
            OnSendMessagesButtonClicked();
            
        }
        OnDisplayMessagesButtonClicked();
    }
    //connect
    public async Task ConnectAsync()
    {
        var localUserData = await _chatClient.ConnectUserAsync("au9ae95vx36j", "longhtl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoibG9uZ2h0bCJ9.2p05WYyOknbhNlX2yttV5ySZAdn2fEqlBv5fcWGLsCE");
        Debug.Log($"User {localUserData.UserId} is connected!");

    }
    //disconnect
    public async Task DisconnectAsync()
    {
        await _chatClient.DisconnectUserAsync();
        Debug.Log($"User disconnected");
    }
    public void OnDisconnectButtonClicked()
    {
        StartCoroutine(DisconnectUser());
        Application.Quit();
    }

    private IEnumerator DisconnectUser()
    {
        yield return DisconnectAsync();
    }
    //send message
    public async Task SendMessageAsync()
    {
        DateTime time = DateTime.Now;
        string timeString=time.ToString("F");

        var channel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, channelName);//2 for me
        Debug.Log($"Connected to channel {channel.Id}");
        var message = await channel.SendNewMessageAsync(timeString + ":"+messageInputField.text+"\n");
        Debug.Log($"Sent message: {message.Text}");
        messageInputField.text = string.Empty;
        OnDisplayMessagesButtonClicked();
    }
    public async void DeleteMessage(IStreamMessage message, bool hardDelete = false)
    {
        if (hardDelete)
        {
            await message.HardDeleteAsync();
        }
        else
        {
            await message.SoftDeleteAsync();
        }
        Debug.Log($"Message deleted: {message.Id}");
    }

    private void OnMessageReceived(IStreamMessage message)
    {
        Debug.Log($"Message received from {message.User.Name}: {message.Text}");
    }

    public void OnSendMessagesButtonClicked()
    {
        StartCoroutine(SendMessage());
    }

    private IEnumerator SendMessage()
    {
        yield return SendMessageAsync();
    }
    //view message (view history)
    public async Task DisplayMessagesAsync()
    {
        var channel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, channelName);
        Debug.Log($"Messages in channel {channel.Id}:");
        _messageText.text = string.Empty;
        _messageText.text = channel.Name+": ";
        foreach (var message in channel.Messages)
        {
            _messageText.text += message.Text+" ";
            Debug.Log($"Message: {message.Text} by {message.User.Id}");
        }
    }
    public void OnDisplayMessagesButtonClicked()
    {
        StartCoroutine(DisplayMessages());
    }

    private IEnumerator DisplayMessages()
    {
        yield return DisplayMessagesAsync();
    }
    //query (not use)
    [System.Obsolete]
    public async Task QueryChannelsAsync()
    {
        var filters = new Dictionary<string, object>
    {
        {
            "members", new Dictionary<string, object>
            {
                { "$in", new string[] { "user-id-to-search" } }
            }
        }
    };
        var channels = await _chatClient.QueryChannelsAsync(filters);

        foreach (var channel in channels)
        {
            _messageText.text += "ID: "+channel.Id +
                "Name: "+channel.Name +
                "Message Count : "+channel.Messages.Count + 
                "Member Count: "+channel.Members.Count;
            Debug.Log(channel.Id);
            Debug.Log(channel.Name);
            Debug.Log(channel.Messages.Count); // Messages
            Debug.Log(channel.Members.Count); // Members
        }
    }

    [Obsolete]
    public void OnQueryChannelsButtonClicked()
    {
        StartCoroutine(QueryChannels());
    }

    [Obsolete]
    private IEnumerator QueryChannels()
    {
        yield return QueryChannelsAsync();
    }
    //like and icon
    // Send simple reaction with a score of 1
    public async Task LikeMessageAsync()
    {
        var channel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, channelName);

        var messageToReact = channel.Messages[0]; // Ví dụ: Lấy tin nhắn đầu tiên
        await messageToReact.SendReactionAsync("like");
        Debug.Log("Sent 'like' reaction.");
    }

    // Hàm gửi phản ứng "clap" với giá trị điểm số tùy chỉnh
    public async Task ClapMessageAsync()
    {
        var channel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, channelName);
        var messageToReact = channel.Messages[0]; // Ví dụ: Lấy tin nhắn đầu tiên
        await messageToReact.SendReactionAsync("clap", 10);
        Debug.Log("Sent 'clap' reaction with score 10.");
    }

    // Hàm gửi phản ứng "love" và thay thế tất cả các phản ứng trước đó từ người dùng này
    public async Task LoveMessageAsync()
    {
        var channel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, channelName);

        var messageToReact = channel.Messages[0]; // Ví dụ: Lấy tin nhắn đầu tiên
        await messageToReact.SendReactionAsync("love", enforceUnique: true);
        Debug.Log("Sent 'love' reaction and enforced uniqueness.");
    }

    public void OnLikeButtonClicked()
    {
        StartCoroutine(LikeMessage());
    }

    private IEnumerator LikeMessage()
    {
        yield return LikeMessageAsync();
    }

    public void OnClapButtonClicked()
    {
        StartCoroutine(ClapMessage());
    }

    private IEnumerator ClapMessage()
    {
        yield return ClapMessageAsync();
    }

    public void OnLoveButtonClicked()
    {
        StartCoroutine(LoveMessage());
    }

    private IEnumerator LoveMessage()
    {
        yield return LoveMessageAsync();
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(DisconnectUser());  
    }
}
