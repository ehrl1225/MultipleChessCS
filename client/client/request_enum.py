from enum import Enum

class RequestEnum(Enum):
    RequestLogin = "RequestLogin"
    RequestRegister = "RequestRegister"
    RequestCreateRoom = "RequestCreateRoom"
    RequestJoinRoom = "RequestJoinRoom"
    RequestDeleteRoom = "RequestDeleteRoom"
    GetRoomList = "GetRoomList"
    StartRoomGame = "StartRoomGame"
    JoinTeam = "JoinTeam"
    SendChat = "SendChat"
    Ping = "Ping"