from src.chat.chat_target import ChatTarget


class Chat:
    def __init__(self, chat_target, sender, message):
        self.__chat_target:ChatTarget = chat_target
        self.__sender = sender
        self.__message = message

    def get_chat_target(self):
        return self.__chat_target

    def get_sender(self):
        return self.__sender

    def get_message(self):
        return self.__message

    def copy(self):
        return Chat(self.__chat_target, self.__sender, self.__message)

    def to_message(self):
        return f"[{ChatTarget(self.__chat_target)}] {self.__sender}: {self.__message}"