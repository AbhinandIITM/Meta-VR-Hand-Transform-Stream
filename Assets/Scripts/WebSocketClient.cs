using NativeWebSocket;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient Instance;

    private WebSocket websocket;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        // Start connection async
        _ = InitializeWebSocket();  // fire-and-forget
    }

    private async Task InitializeWebSocket()
    {
        websocket = new WebSocket("wss://next-magical-viper.ngrok-free.app");


        websocket.OnOpen += () =>
        {
            Debug.Log("✅ WebSocket Opened");
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError("❌ WebSocket Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("🔌 WebSocket Closed");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("📩 Message Received: " + message);
        };

        await websocket.Connect();
    }

    public void SendJson(string json)
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            _ = websocket.SendText(json); // fire-and-forget, avoid async void
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
#endif
    }

    void OnApplicationQuit()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket != null)
        {
            _ = websocket.Close(); // fire-and-forget
        }
#endif
    }
}
