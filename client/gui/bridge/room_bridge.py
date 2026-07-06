from unittest import case

from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

from chess.chess_team import ChessTeam
from client.HubAction import HubAction
from client.chess_hub_interface import IChessHub
from client.response_enum import ResponseEnum
from client.request_enum import RequestEnum

class RoomBridge(QObject):
    roomInfoChanged = pyqtSignal()
    gameStarted = pyqtSignal()
    leftRoom = pyqtSignal()

    def __init__(self, signalr_client: IChessHub):
        super().__init__()
        self.signalr_client = signalr_client

        self._room_id = ""
        self._room_name = "대기실"
        self._players = []
        self._is_game_started = False
        self._is_host = False
        self._my_username = ""

        self._add_handlers()

    def _add_handlers(self):
        self.signalr_client.addHandler(ResponseEnum.HubResponse, self._on_hub_response)
        self.signalr_client.addHandler(ResponseEnum.GroupNotice, self._on_group_notice)
        self.signalr_client.addHandler(ResponseEnum.ChessRoomDetailResponse, self._on_chess_room_detail)

    def _on_hub_response(self, args):
        hub_action: int = args[0]
        success: bool = args[1]
        msg: str = args[2]
        match hub_action:
            case HubAction.JoinTeam:
                if success:
                    pass


    def _on_chess_room_detail(self, args):
        if not args: return
        data = args[0]
        self._room_id = data.get("roomId", self._room_id)
        self._room_name = data.get("roomName", self._room_name)
        self._players = data.get("players", [])
        self._is_game_started = data.get("isStarted", False)
        self.roomInfoChanged.emit()

    def _on_group_notice(self, args):
        if not args: return
        msg = args[0]

    @pyqtProperty(str, notify=roomInfoChanged)
    def roomName(self): return self._room_name

    @pyqtProperty(list, notify=roomInfoChanged)
    def players(self): return self._players

    @pyqtProperty(list, notify=roomInfoChanged)
    def viewers(self): return [player for player in self._players if player.get("team") == ChessTeam.Viewer]

    @pyqtProperty(list, notify=roomInfoChanged)
    def black_teams(self): return [player for player in self._players if player.get("team") == ChessTeam.Black]

    @pyqtProperty(list, notify=roomInfoChanged)
    def white_teams(self): return [player for player in self._players if player.get("team") == ChessTeam.White]

    @pyqtProperty(bool, notify=roomInfoChanged)
    def isGameStarted(self): return self._is_game_started

    @pyqtProperty(bool, notify=roomInfoChanged)
    def isHost(self): return self._is_host

    @pyqtProperty(bool, notify=roomInfoChanged)
    def canStart(self):
        return len(self._players) >= 2

    @pyqtSlot(str, str)
    def changeTeam(self, room_id: str, team_name: str):
        self.signalr_client.join_team(room_id, team_name)

    @pyqtSlot(str)
    def joinTeam(self, team_name: str):
        team = ChessTeam.from_str(team_name)
        if team is None:
            return
        self.signalr_client.join_team(team)

    @pyqtSlot(str)
    def leaveTeam(self, room_id: str):
        self.signalr_client.leave_team(room_id)

    @pyqtSlot()
    def getRoomInfo(self):
        self.signalr_client.get_room_info()

    @pyqtSlot()
    def leaveRoom(self):
        self.signalr_client.request_delete_room(self._room_id)



