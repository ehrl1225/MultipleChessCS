from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

from src.client.HubAction import HubAction
from src.client.chess_hub_interface import IChessHub
from src.client.response_enum import ResponseEnum


class LobbyBridge(QObject):
    roomListChanged = pyqtSignal(list)
    createRoomSuccess = pyqtSignal()
    joinRoomSuccess = pyqtSignal()
    
    def __init__(self, signalr_client: IChessHub):
        super().__init__()
        self.signalr_client:IChessHub = signalr_client
        self.add_handler()
        self._rooms = []

    def add_handler(self):
        self.signalr_client.addHandler(ResponseEnum.HubResponse, self.onHubResponse)
        self.signalr_client.addHandler(ResponseEnum.ChessRoomListResponse, self.onRoomListResponse)

    def onHubResponse(self, args):
        hub_action: int = args[0]
        success: bool = args[1]
        msg: str = args[2]
        if not success:
            return

        match hub_action:
            case HubAction.CreateRoom:
                self.createRoomSuccess.emit()
            case HubAction.JoinRoom:
                self.joinRoomSuccess.emit()


    def onRoomListResponse(self, args):
        # args expected format: [rooms_data]
        # rooms_data might be a list of dicts: [{"roomId": 1, "roomName": "Room 1", "playerCount": 1}, ...]
        self._rooms = args[0]
        self.roomListChanged.emit(self._rooms)

    @pyqtSlot()
    def refreshRoomList(self):
        self.signalr_client.get_room_list()

    @pyqtSlot(str, int)
    def createRoom(self, room_name: str, max_player_count: int):
        self.signalr_client.request_create_room(room_name, max_player_count)

    @pyqtSlot(str)
    def joinRoom(self, room_id):
        self.signalr_client.request_join_room(room_id)

    @pyqtProperty(list, notify=roomListChanged)
    def rooms(self):
        return self._rooms