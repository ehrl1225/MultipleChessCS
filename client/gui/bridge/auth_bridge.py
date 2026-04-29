from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

from client.chess_hub_interface import IChessHub
from client.response_enum import ResponseEnum
from client.signalr_client import SignalRClient


class AuthBridge(QObject):
    loginSuccess = pyqtSignal()
    errorOccurred = pyqtSignal(str)

    def __init__(self, signalr_client:IChessHub):
        super().__init__()
        self.signalr_client = signalr_client
        self._error_message = ""

    def add_handler(self):
        self.signalr_client.addHandler(ResponseEnum.RegisterResponse.value, self.onRegisterResponse)
        self.signalr_client.addHandler(ResponseEnum.LoginResponse.value, self.onLoginResponse)

    def onRegisterResponse(self, args):
        success = args[0]
        message = args[1]


    def onLoginResponse(self, args):
        success: bool = args[0]
        message: str = args[1]
        if success:
            self.loginSuccess.emit()
        else:
            self._error_message = message
            self.errorOccurred.emit(message)

    @pyqtProperty(str, notify=errorOccurred)
    def errorMessage(self):
        return self._error_message

    @pyqtSlot(str, str)
    def login(self, username, password):
        self.signalr_client.request_login(username, password)
        # TODO 인증 로직 구현

    @pyqtSlot(str, str, str)
    def register(self, username, password):
        pass
        # TODO 회원가입 구현