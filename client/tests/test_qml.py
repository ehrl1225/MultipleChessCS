import sys
import os
from PyQt6.QtGui import QGuiApplication
from PyQt6.QtQml import QQmlApplicationEngine
from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

class DummyBridge(QObject):
    pass

def main():
    app = QGuiApplication(sys.argv)
    engine = QQmlApplicationEngine()

    bridge = DummyBridge()
    engine.rootContext().setContextProperty("bridge", bridge)
