import asyncio
import pytest
import logging
from signalrcore.hub_connection_builder import HubConnectionBuilder
from signalrcore.hub.base_hub_connection import BaseHubConnection

def make_connection() -> BaseHubConnection:
    url = "http://127.0.0.1:5000/chess_hub"
    return (
        HubConnectionBuilder()
        .with_url(url)
        .configure_logging(logging.DEBUG)
        .with_automatic_reconnect({
            "type": "raw",
            "keep_alive_interval": 10,
            "reconnect_interval": 5,
            "max_attempts": 5,
        })
        .build()
    )

@pytest.mark.asyncio
async def test_ping_pong():
    print()
    pong_received = asyncio.get_running_loop().create_future()
    loop = asyncio.get_running_loop()

    connection:BaseHubConnection = make_connection()
    def on_open():
        print("Connection opened")
        connection.send("Ping", ["ping"])

    def on_pong(args):
        msg = args[0]
        print(f"received msg : {msg}")
        loop.call_soon_threadsafe(pong_received.set_result, msg)

    connection.on_open(on_open)
    connection.on_close(lambda : print("Connection closed"))

    connection.on("Pong", on_pong)
    connection.start()
    try:
        result = await asyncio.wait_for(pong_received, timeout=5)
        assert result == "Pong"
    except asyncio.TimeoutError:
        print("Connection timed out")
        assert False, "Connection timed out"
