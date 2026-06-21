from enum import IntEnum

class HubAction(IntEnum):
    Register = 0
    Login = 1
    CreateRoom = 2
    JoinRoom = 3
    DeleteRoom = 4
    LeaveRoom = 5
    JoinTeam = 6
    LeaveTeam = 7