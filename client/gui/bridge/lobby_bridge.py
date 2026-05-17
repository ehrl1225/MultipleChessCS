from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

from client.chess_hub_interface import IChessHub
from client.response_enum import ResponseEnum


class LobbyBridge(QObject):
    roomListChanged = pyqtSignal(list)
    joinRoomSuccess = pyqtSignal()
    
    def __init__(self, signalr_client: IChessHub):
        super().__init__()
        self.signalr_client:IChessHub = signalr_client
        self.add_handler()
        self._rooms = []

    def add_handler(self):
        self.signalr_client.addHandler(ResponseEnum.ChessRoomListResponse.value, self.onRoomListResponse)

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

    @pyqtSlot(int)
    def joinRoom(self, room_id):
        self.signalr_client.request_join_room(room_id)
