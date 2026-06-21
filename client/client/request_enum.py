from enum import Enum

class RequestEnum(Enum):
    # auth
    RequestLogin = "RequestLogin"
    RequestRegister = "RequestRegister"

    # room
    RequestCreateRoom = "RequestCreateRoom"
    RequestJoinRoom = "RequestJoinRoom"
    RequestDeleteRoom = "RequestDeleteRoom"
    GetRoomList = "GetRoomList"
    StartRoomGame = "StartRoomGame"

    # team
    JoinTeam = "JoinTeam"
    LeaveTeam = "LeaveTeam"

    # etc
    SendChat = "SendChat"
    Ping = "Ping"