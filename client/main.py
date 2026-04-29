import os
import sys
from PyQt6.QtGui import QGuiApplication
from PyQt6.QtQml import QQmlApplicationEngine

from chess.chess_board import ChessBoard
from gui.bridge import AuthBridge, ChessBoardBridge
from client.signalr_client import SignalRClient


def main():
    os.environ["QT_QUICK_CONTROLS_STYLE"] = "Basic"
    app = QGuiApplication(sys.argv)
    engine = QQmlApplicationEngine()
    signalr_client = SignalRClient("http://127.0.0.1:5000")
    chessBoard = ChessBoard()
    bridge = ChessBoardBridge(chessBoard)
    login_view_bridge = AuthBridge(signalr_client)
    engine.rootContext().setContextProperty("bridge", bridge)
    engine.rootContext().setContextProperty("authBridge", login_view_bridge)

    engine.load("gui/qml/Main.qml")
    sys.exit(app.exec())

if __name__ == "__main__":
    main()
