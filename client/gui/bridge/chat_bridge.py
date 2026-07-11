from PyQt6.QtCore import QObject, pyqtProperty

from chat.chat_list import ChatList
from client.HubAction import HubAction
from client.chess_hub_interface import IChessHub
from client.response_enum import ResponseEnum


class ChatBridge(QObject):

    def __init__(self, signalr_client: IChessHub):
        super().__init__()
        self.chat_list = ChatList()
        self.signalr_client = signalr_client
        self.add_handler()

    def add_handler(self):
        self.signalr_client.addHandler(ResponseEnum.HubResponse, self.onHubResponse)

    def onHubResponse(self, args):
        hub_action: int = args[0]
        success: bool = args[1]
        msg: str = args[2]

        match hub_action:
            case HubAction.SendChat:
                pass


