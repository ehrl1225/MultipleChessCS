import QtQuick 2.15
import QtQuick.Window 2.15
import QtQuick.Controls

ApplicationWindow {
    id: window
    visible: true
    width: 900
    height: 700
    title: qsTr("MultipleChess")

    StackView {
        id: mainStack
        anchors.fill: parent
        initialItem: LoginView {}
    }

    Connections {
        target: authBridge
        function onLoginSuccess() {
            mainStack.replace("LobbyView.qml")
        }
        function onRegisterSuccess() {
            mainStack.pop()
        }
    }
}