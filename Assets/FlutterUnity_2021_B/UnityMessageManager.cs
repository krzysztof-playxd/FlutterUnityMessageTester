using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityMessageManager : MonoBehaviour
{
    #region SINGLETON
    private static UnityMessageManager _instance;

    public static UnityMessageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UnityMessageManager>();

                if (_instance == null)
                {
                    Debug.Log("Creating new instance in prop");
                    GameObject ummgo = new GameObject("UnityMessageManager");
                    _instance = ummgo.AddComponent<UnityMessageManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else if (_instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public const string MessagePrefix = "@UnityMessage@";
    private static int ID = 0;

    private static int generateId()
    {
        ID = ID + 1;
        return ID;
    }

    public delegate void MessageDelegate(string message);
    public event MessageDelegate OnMessage;

    public delegate void MessageHandlerDelegate(MessageHandler handler);
    public event MessageHandlerDelegate OnFlutterMessage;

    private Dictionary<int, UnityMessage> waitCallbackMessageMap = new Dictionary<int, UnityMessage>();

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        NativeAPI.OnSceneLoaded(scene, mode);
    }

    public void ShowHostMainWindow()
    {
        NativeAPI.ShowHostMainWindow();
    }

    public void UnloadMainWindow()
    {
        NativeAPI.UnloadMainWindow();
    }

    public void QuitUnityWindow()
    {
        NativeAPI.QuitUnityWindow();
    }

    public void SendMessageToFlutter(string message)
    {
        NativeAPI.SendMessageToFlutter(message);
        UIManager.Instance.SendMessageToFlutter(message);
    }

    public void SendMessageToFlutter(UnityMessage message)
    {
        int id = generateId();
        if (message.callBack != null)
        {
            waitCallbackMessageMap.Add(id, message);
        }

        JObject o = JObject.FromObject(new
        {
            id = id,
            seq = message.callBack != null ? "start" : "",
            name = message.name,
            data = message.data
        });
        UnityMessageManager.Instance.SendMessageToFlutter(MessagePrefix + o.ToString());
    }

    void onMessage(string message)
    {
        if (OnMessage != null)
        {
            OnMessage(message);
        }
    }

    void onFlutterMessage(string message)
    {
        if (message.StartsWith(MessagePrefix))
        {
            message = message.Replace(MessagePrefix, "");
        }
        else
        {
            return;
        }

        MessageHandler handler = MessageHandler.Deserialize(message);
        if ("end".Equals(handler.seq))
        {
            // handle callback message
            UnityMessage m;
            if (waitCallbackMessageMap.TryGetValue(handler.id, out m))
            {
                waitCallbackMessageMap.Remove(handler.id);
                if (m.callBack != null)
                {
                    m.callBack(handler.getData<object>()); // todo
                }
            }
            return;
        }

        if (OnFlutterMessage != null)
        {
            OnFlutterMessage(handler);
            UIManager.Instance.ReceiveMessageFromFlutter(message);
        }
    }
}