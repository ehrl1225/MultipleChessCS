import QtQuick 2.15
import QtQuick.Window 2.15
import QtQuick.Controls
import QtQuick.Layouts 1.15

ApplicationWindow {
    id: window
    visible: true
    width: isLoggedIn ? 1200 : 900
    height: 700
    title: qsTr("MultipleChess")

    property bool isLoggedIn: false

    Row {
        anchors.fill: parent

        StackView {
            id: mainStack
            width: isLoggedIn ? parent.width - 300 : parent.width
            height: parent.height
            initialItem: LoginView {}
        }

        Rectangle {
            id: chatArea
            width: 300
            height: parent.height
            color: "#f8f9fa"
            visible: isLoggedIn
            border.color: "#dee2e6"
            border.width: 1

            Column {
                anchors.fill: parent
                anchors.margins: 10
                spacing: 10

                Text {
                    text: "실시간 채팅"
                    font.bold: true
                    font.pixelSize: 16
                }

                Rectangle {
                    width: parent.width
                    height: parent.height - 80
                    color: "white"
                    border.color: "#e9ecef"
                    
                    Text {
                        anchors.centerIn: parent
                        text: "[채팅 내용 표시 영역]"
                        color: "#adb5bd"
                    }
                }

                RowLayout {
                    width: parent.width
                    spacing: 5

                    ComboBox {
                        id: chatTargetCombo
                        implicitWidth: 80
                        model: ["모두", "방", "팀"]
                        currentIndex: 0
                    }

                    TextField {
                        id: messageInput
                        Layout.fillWidth: true
                        placeholderText: "메시지를 입력하세요..."
                        onAccepted: {

                            messageInput.text = "";
                        }
                    }
                }
            }
        }
    }

    Connections {
        target: authBridge
        function onLoginSuccess() {
            isLoggedIn = true;
            mainStack.replace("LobbyView.qml");
            lobbyBridge.refreshRoomList();
        }
        function onRegisterSuccess() {
            mainStack.pop();
        }
    }

    Connections {
        target: lobbyBridge
        function onCreateRoomSuccess() {
            mainStack.push("ChessRoomView.qml");
            roomBridge.getRoomInfo();
        }

        function onJoinRoomSuccess() {
            mainStack.push("ChessRoomView.qml");
            roomBridge.getRoomInfo();
        }
    }
}