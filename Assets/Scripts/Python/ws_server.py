import asyncio
import websockets
import datetime

PORT = 8080

async def echo(websocket):
    print(f"🟢 Client connected at {datetime.datetime.now()}")

    try:
        async for message in websocket:
            print(f"📨 Received: {message}")
            await websocket.send(f"Echo: {message}")
    except websockets.exceptions.ConnectionClosed:
        print("🔴 Client disconnected")

async def main():
    async with websockets.serve(echo, "0.0.0.0", PORT):
        print(f"✅ WebSocket server started on ws://localhost:{PORT}")
        await asyncio.Future()  # run forever

if __name__ == "__main__":
    asyncio.run(main())
