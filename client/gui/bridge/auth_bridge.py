from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

from client.HubAction import HubAction
from client.chess_hub_interface import IChessHub
from client.response_enum import ResponseEnum
from client.signalr_client import SignalRClient
from gui.user_data import UserData


class AuthBridge(QObject):
    loginSuccess = pyqtSignal()
    registerSuccess = pyqtSignal()
    errorOccurred = pyqtSignal(str)

    def __init__(self, signalr_client:IChessHub, userdata: UserData):
        super().__init__()
        self.signalr_client = signalr_client
        self.userdata = userdata
        self.add_handler()
        self._error_message = ""
        self.__username = ""

    def add_handler(self):
        self.signalr_client.addHandler(ResponseEnum.HubResponse, self.onHubResponse)

    def onHubResponse(self, args):
        hub_action: int = args[0]
        success: bool = args[1]
        msg: str = args[2]
        if not success:
            self._error_message = msg
            self.errorOccurred.emit(msg)
            self.userdata.setUsername(self.__username)
            return
        match hub_action:
            case HubAction.Register:
                self.registerSuccess.emit()
            case HubAction.Login:
                self.loginSuccess.emit()


    @pyqtProperty(str, notify=errorOccurred)
    def errorMessage(self):
        return self._error_message

    @pyqtSlot(str, str)
    def login(self, username, password):
        print("로그인 시도")
        self.signalr_client.request_login(username, password)
        self.__username = username

    @pyqtSlot(str, str)
    def register(self, username, password):
        print("회원가입 시도")
        self.signalr_client.request_register(username, password)