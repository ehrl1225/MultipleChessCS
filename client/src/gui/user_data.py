from typing import Optional


class UserData:
    __username: Optional[str] = None

    def __init__(self):
        pass

    def setUsername(self, username):
        self.__username = username

    def getUsername(self):
        return self.__username