# Meta VR Hand Transform Stream

This Unity project streams hand transform data over a WebSocket connection, enabling real-time communication between a Unity application and external services.

## Features

- **WebSocket Client**: Uses [`WebSocketClient`](Assets/Scripts/WebSocketClient.cs) to connect to a remote WebSocket server.
- **Singleton Pattern**: Ensures only one instance of the WebSocket client exists.
- **Automatic Reconnection**: Initializes the WebSocket connection on startup.
- **JSON Messaging**: Easily send JSON-formatted data to the server.
- **Unity Integration**: Designed to work seamlessly with Unity's update loop and lifecycle events.

## Getting Started

### Prerequisites

- Unity 2020.3 or newer (recommended)
- [NativeWebSocket](https://github.com/endel/NativeWebSocket) package (already included via `endel.nativewebsocket.csproj`)

## Setup

1. **Start the WebSocket Server**  
   Run your Python WebSocket server script:
   ```sh
   python ws_server.py
   ```

2. **Start ngrok**  
   Expose your local server to the internet:
   ```sh
   ngrok http 8080
   ```
   

3. **Run Unity**  
   Open the project in Unity and press Play. The WebSocket client will automatically connect to the ngrok URL.

## File Structure

- [`Assets/Scripts/WebSocketClient.cs`](Assets/Scripts/WebSocketClient.cs): Main WebSocket client logic.
- [`Assets/TutorialInfo/`](Assets/TutorialInfo/): Unity tutorial and documentation assets.

## Customization

- **Change WebSocket URL**: Edit the URL in [`WebSocketClient`](Assets/Scripts/WebSocketClient.cs) constructor.
- **Handle Messages**: Modify the `websocket.OnMessage` callback to process incoming data as needed.

## License

See [LICENSE](LICENSE) for details.

## Acknowledgements

- [NativeWebSocket](https://github.com/endel/NativeWebSocket) by Endel Dreyer

---
