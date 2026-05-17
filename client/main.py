import os
import sys
from PyQt6.QtGui import QGuiApplication
from PyQt6.QtQml import QQmlApplicationEngine

from chess.chess_board import ChessBoard
from gui.bridge import AuthBridge, ChessBoardBridge
from client.signalr_client import SignalRClient
from gui.bridge.lobby_bridge import LobbyBridge


def main():
    os.environ["QT_QUICK_CONTROLS_STYLE"] = "Basic"
    app = QGuiApplication(sys.argv)
    engine = QQmlApplicationEngine()
    signalr_client = SignalRClient("http://127.0.0.1:5000/chess_hub")
    chessBoard = ChessBoard()
    bridge = ChessBoardBridge(chessBoard)
    auth_bridge = AuthBridge(signalr_client)
    lobby_bridge = LobbyBridge(signalr_client)
    engine.rootContext().setContextProperty("bridge", bridge)
    engine.rootContext().setContextProperty("authBridge", auth_bridge)
    engine.rootContext().setContextProperty("lobbyBridge", lobby_bridge)

    engine.load("gui/qml/Main.qml")
    signalr_client.connect()
    sys.exit(app.exec())

if __name__ == "__main__":
    main()
