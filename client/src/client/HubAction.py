from enum import IntEnum

class HubAction(IntEnum):
    # 000대 : Account / User 관련 액션
    Register = 1
    Login = 2
    Logout = 3

    # 100대 : Room 관련 액션
    CreateRoom = 101
    JoinRoom = 102
    DeleteRoom = 103
    LeaveRoom = 104
    GetRoomList = 105

    # 200대 : Team 관련 액션
    JoinTeam = 201
    LeaveTeam = 202

    # 900대 : 기타 액션
    SendChat = 901