from enum import Enum

class ResponseEnum(Enum):
    RegisterResponse = "RegisterResponse"
    LoginResponse = "LoginResponse"
    GroupNotice = "GroupNotice"
    CallerMessage = "CallerMessage"
    SendMessage = "SendMessage"