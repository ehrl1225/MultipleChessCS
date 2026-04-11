from abc import ABCMeta, abstractmethod


class ChessHubInterface(metaclass=ABCMeta):

    @abstractmethod
    def _register_response(self, success: bool, message: str):
        pass
