from unittest import case

from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

from src.chat.chat_target import ChatTarget
from src.chat.chat_list import ChatList
from src.client.HubAction import HubAction
from src.client.chess_hub_interface import IChessHub
from src.client.response_enum import ResponseEnum


class ChatBridge(QObject):
    chat_list_changed = pyqtSignal()

    def __init__(self, signalr_client: IChessHub):
        super().__init__()
        self.chat_list = ChatList()
        self.signalr_client = signalr_client
        self.add_handler()

    def add_handler(self):
        self.signalr_client.addHandler(ResponseEnum.HubResponse, self.onHubResponse)
        self.signalr_client.addHandler(ResponseEnum.SendMessage, self.onSendMessage)

    def onSendMessage(self, args):
        chat_target: ChatTarget = args[0]
        sender: str = args[1]
        message: str = args[2]
        self.chat_list.add_chat(chat_target, sender, message)
        self.chat_list_changed.emit()

    def onHubResponse(self, args):
        hub_action: HubAction = args[0]
        success: bool = args[1]
        msg: str = args[2]

        match hub_action:
            case HubAction.SendChat:
                pass

    @pyqtProperty(list, notify=chat_list_changed)
    def messages(self) -> list:
        return self.chat_list.get_messages()

    @pyqtSlot(int, str)
    def send_message(self, chat_target:ChatTarget, message: str):
        self.signalr_client.send_chat(chat_target, message)
