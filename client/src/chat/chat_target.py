from enum import IntEnum

class ChatTarget(IntEnum):
    ALL = 0
    ROOM = 1
    TEAM = 2

    def __str__(self):
        match self.value:
            case ChatTarget.ALL:
                return "전체"
            case ChatTarget.ROOM:
                return "방"
            case ChatTarget.TEAM:
                return "팀"
        return ""