using StreamChat.Core;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
public class ChatManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro _messageText;
    [SerializeField] private TMP_InputField messageInputField;
    private IStreamChatClient _chatClient;
    private async void Start()
    {
        _chatClient = StreamChatClient.CreateDefaultClient();
        await ConnectAsync();  // Tự động kết nối khi khởi động ứng dụng
    }
    public async Task ConnectAsync()
    {
        var localUserData = await _chatClient.ConnectUserAsync("au9ae95vx36j", "longhtl", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoibG9uZ2h0bCJ9.2p05WYyOknbhNlX2yttV5ySZAdn2fEqlBv5fcWGLsCE");
        Debug.Log($"User {localUserData.UserId} is connected!");

    }
    public async Task DisconnectAsync()
    {
        await _chatClient.DisconnectUserAsync();
        Debug.Log($"User disconnected");
    }
    
    public async Task SendMessageAsync()
    {
        var channel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, "1326465");
        Debug.Log($"Connected to channel {channel.Id}");
        var message = await channel.SendNewMessageAsync("Hello");
        Debug.Log($"Sent message: {message.Text}");
    }
    public void OnSendMessagesButtonClicked()
    {
        StartCoroutine(SendMessage());
    }

    private IEnumerator SendMessage()
    {
        yield return SendMessageAsync();
    }
    public async Task DisplayMessagesAsync()
    {
        var channel = await _chatClient.GetOrCreateChannelWithIdAsync(ChannelType.Messaging, "1326465");
        Debug.Log($"Messages in channel {channel.Id}:");

        foreach (var message in channel.Messages)
        {
            Debug.Log($"Message: {message.Text} by {message.User.Id}");
        }
    }

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
            Debug.Log(channel.Id);
            Debug.Log(channel.Name);
            Debug.Log(channel.Messages.Count); // Messages
            Debug.Log(channel.Members.Count); // Members
        }
    }
    public void OnDisconnectButtonClicked()
    {
        StartCoroutine(DisconnectUser());
    }

    private IEnumerator DisconnectUser()
    {
        yield return DisconnectAsync();
    }
    public void OnDisplayMessagesButtonClicked()
    {
        StartCoroutine(DisplayMessages());
    }

    private IEnumerator DisplayMessages()
    {
        yield return DisplayMessagesAsync();
    }
    public void OnQueryChannelsButtonClicked()
    {
        StartCoroutine(QueryChannels());
    }

    private IEnumerator QueryChannels()
    {
        yield return QueryChannelsAsync();
    }

}
