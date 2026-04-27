from PyQt6.QtCore import QObject, pyqtProperty, pyqtSignal, pyqtSlot

class LoginViewBridge(QObject):
    loginSuccess = pyqtSignal()
    errorOccurred = pyqtSignal(str)

    def __init__(self):
        super().__init__()
        self._error_message = ""

    @pyqtProperty(str, notify=errorOccurred)
    def errorMessage(self):
        return self._error_message

    @pyqtSlot(str, str)
    def login(self, username, password):
        pass
        # TODO 인증 로직 구현

    @pyqtSlot(str, str, str)
    def register(self, username, password, nickname):
        pass
        # TODO 회원가입 구현