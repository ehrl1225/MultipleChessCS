from enum import StrEnum

class ResponseEnum(StrEnum):
    RegisterResponse = "RegisterResponse"
    LoginResponse = "LoginResponse"
    GroupNotice = "GroupNotice"
    SendMessage = "SendMessage"
    Pong = "Pong"
    Alert = "Alert"
    ChessRoomListResponse = "ChessRoomListResponse"
    HubResponse = "HubResponse"