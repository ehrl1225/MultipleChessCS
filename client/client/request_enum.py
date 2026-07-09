from enum import StrEnum

class RequestEnum(StrEnum):
    # auth
    RequestLogin = "RequestLogin"
    RequestRegister = "RequestRegister"

    # room
    RequestCreateRoom = "RequestCreateRoom"
    RequestJoinRoom = "RequestJoinRoom"
    RequestDeleteRoom = "RequestDeleteRoom"
    GetRoomList = "GetRoomList"
    StartRoomGame = "StartRoomGame"
    GetRoomInfo = "GetRoomInfo"

    # team
    JoinTeam = "JoinTeam"
    LeaveTeam = "LeaveTeam"

    # etc
    SendChat = "SendChat"
    Ping = "Ping"