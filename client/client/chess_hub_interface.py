from abc import ABCMeta, abstractmethod
from typing import Callable, Any


class IChessHub(metaclass=ABCMeta):

    @abstractmethod
    def addHandler(self, event_name: str, handler: Callable[[Any], None]):
        pass

    @abstractmethod
    def request_register(self, username: str, password: str):
        pass

    @abstractmethod
    def request_login(self, username:str, password:str):
        pass
