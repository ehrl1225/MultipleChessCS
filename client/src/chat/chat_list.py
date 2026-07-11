from src.chat.chat import Chat
from src.chat.chat_target import ChatTarget


class ChatList:

    def __init__(self):
        self.__chat_list:list[Chat] = []

    def add_chat(self,chat_target:ChatTarget, sender: str, message: str):
        chat = Chat(chat_target, sender, message)
        self.__chat_list.append(chat)
        return chat

    def get_chat_list(self):
        """
        실제 채팅의 복사본을 반환합니다.
        따라서 반환값을 수정해도 반영되지 않습니다.
        """
        return [chat.copy() for chat in self.__chat_list]

    def get_chat_by_target(self, chat_target:ChatTarget):
        """
        ChatTarget에 맞는 채팅의 복사본을 반환합니다.
        따라서 반환값을 수정해도 반영되지 않습니다.
        """
        arr = []
        for chat in self.__chat_list:
            target = chat.get_chat_target()
            if target == chat_target:
                arr.append(chat.copy())
        return arr