import os
import sys
from PyQt6.QtGui import QGuiApplication
from PyQt6.QtQml import QQmlApplicationEngine

from src.chess.chess_board import ChessBoard
from src.gui.bridge import AuthBridge, ChessBoardBridge
from src.client.signalr_client import SignalRClient
from src.gui.bridge.lobby_bridge import LobbyBridge
from src.gui.bridge.room_bridge import RoomBridge
from src.gui.user_data import UserData


def main():
    os.environ["QT_QUICK_CONTROLS_STYLE"] = "Basic"
    app = QGuiApplication(sys.argv)
    engine = QQmlApplicationEngine()
    signalr_client = SignalRClient("http://127.0.0.1:5000/chess_hub")
    chessBoard = ChessBoard()
    userdata = UserData()
    bridge = ChessBoardBridge(chessBoard)
    auth_bridge = AuthBridge(signalr_client, userdata)
    lobby_bridge = LobbyBridge(signalr_client)
    room_bridge = RoomBridge(signalr_client, userdata)
    engine.rootContext().setContextProperty("bridge", bridge)
    engine.rootContext().setContextProperty("authBridge", auth_bridge)
    engine.rootContext().setContextProperty("lobbyBridge", lobby_bridge)
    engine.rootContext().setContextProperty("roomBridge", room_bridge)

    engine.load("src/gui/qml/Main.qml")
    result = signalr_client.connect()
    if result:
        sys.exit(app.exec())
    else:
        print("Can't connect to server")

if __name__ == "__main__":
    main()
