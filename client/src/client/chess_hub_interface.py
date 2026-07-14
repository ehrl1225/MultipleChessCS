from abc import ABCMeta, abstractmethod
from typing import Callable, Any

from chat.chat_target import ChatTarget


class IChessHub(metaclass=ABCMeta):

    @abstractmethod
    def addHandler(self, event_name: str, handler: Callable[[Any], None]): pass

    @abstractmethod
    def request_register(self, username: str, password: str): pass

    @abstractmethod
    def request_login(self, username:str, password:str): pass

    @abstractmethod
    def request_create_room(self, room_name: str, max_player_count: int): pass

    @abstractmethod
    def request_join_room(self, room_id: str): pass

    @abstractmethod
    def request_delete_room(self, room_id: str): pass

    @abstractmethod
    def get_room_list(self): pass

    @abstractmethod
    def get_room_info(self): pass

    @abstractmethod
    def join_team(self, team: int): pass

    @abstractmethod
    def leave_team(self, room_id: str): pass

    @abstractmethod
    def send_chat(self, chat_target: ChatTarget, message: str): pass



