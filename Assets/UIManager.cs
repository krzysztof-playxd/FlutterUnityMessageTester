using System;
using Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button _clearSentMessagesButton;

    [SerializeField]
    private Button _clearReceivedMessagesButton;

    [SerializeField]
    private Toggle _keepMessageToggle;

    [SerializeField]
    private Button _sendToFlutterButton;

    [SerializeField]
    private TMP_InputField _inputField;

    [SerializeField]
    private TextMeshProUGUI _placeholderText;

    [SerializeField]
    private TextMeshProUGUI _sentMessagesText;

    [SerializeField]
    private TextMeshProUGUI _receivedMessagesText;

    private int _flag = 0;

    #region SINGLETON
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(this);
    }

    public void SendMessageToFlutter(string message)
    {
        AppendSentMessage(message);
    }

    public void ReceiveMessageFromFlutter(string message)
    {
        AppendReceiveMessage(message);
    }
    #endregion

    private void Start()
    {
        _clearSentMessagesButton.onClick.AddListener(OnClearSentMessagesButton);
        _clearReceivedMessagesButton.onClick.AddListener(OnClearReceivedMessagesButton);
        _sendToFlutterButton.onClick.AddListener(OnSendToFlutterButton);

        OnClearSentMessagesButton();
        OnClearReceivedMessagesButton();
    }

    private void OnClearSentMessagesButton()
    {
        _sentMessagesText.text = string.Empty;
    }

    private void OnClearReceivedMessagesButton()
    {
        _receivedMessagesText.text = string.Empty;
    }

    private void OnSendToFlutterButton()
    {
        var message = _inputField.text;
        if (string.IsNullOrWhiteSpace(message))
        {
            var msg = string.Empty;
            if (_flag == 0) msg = "Forgot to put message?";
            if (_flag == 1) msg = ">>> Input message here <<<";
            if (_flag == 2) msg = "Halo! Please input here";
            if (_flag == 3) msg = "No, not without message";
            if (_flag == 4) msg = "Really? Please insert msg first!";
            _flag++;
            if (_flag >= 5) _flag = 0;
            _placeholderText.text = msg;
            return;
        }

        //Clear message field?
        if (_keepMessageToggle.isOn == false)
        {
            _inputField.text = string.Empty;
            _placeholderText.text = "Enter message...";
        }
        
        FlutterMessengerWrapper.Send(message);
    }

    private void AppendSentMessage(string message)
    {
        var newMessage = $"{GetDateTimePrefix()}{message}{Environment.NewLine}";
        _sentMessagesText.text = newMessage + _sentMessagesText.text;
    }

    private void AppendReceiveMessage(string message)
    {
        var newMessage = $"{GetDateTimePrefix()}{message}{Environment.NewLine}";
        _sentMessagesText.text = newMessage + _sentMessagesText.text;
    }

    private string GetDateTimePrefix()
    {
        var dt = DateTime.Now;
        return $"[{dt:HH:mm:ss.fff}] -> ";
    }
}
