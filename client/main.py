import os
import sys
from PyQt6.QtGui import QGuiApplication
from PyQt6.QtQml import QQmlApplicationEngine

from chess.chess_board import ChessBoard
from gui.bridge.login_view_bridge import LoginViewBridge
from gui.chess_bridge import ChessBridge


def main():
    os.environ["QT_QUICK_CONTROLS_STYLE"] = "Basic"
    app = QGuiApplication(sys.argv)
    engine = QQmlApplicationEngine()
    chessBoard = ChessBoard()
    bridge = ChessBridge(chessBoard)
    loginViewBridge = LoginViewBridge()
    engine.rootContext().setContextProperty("bridge", bridge)
    engine.rootContext().setContextProperty("LoginViewBridge", loginViewBridge)

    engine.load("gui/qml/Main.qml")
    sys.exit(app.exec())

if __name__ == "__main__":
    main()
