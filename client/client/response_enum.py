from enum import Enum

class ResponseEnum(Enum):
    RegisterResponse = "RegisterResponse"
    LoginResponse = "LoginResponse"
    GroupNotice = "GroupNotice"
    SendMessage = "SendMessage"
    Pong = "Pong"
    Alert = "Alert"
    ChessRoomListResponse = "ChessRoomListResponse"