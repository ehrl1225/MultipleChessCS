import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15

Rectangle {
    id: lobbyRoot
    color: "#f0f2f5"

    ColumnLayout {
        anchors.fill: parent
        anchors.margins: 20
        spacing: 20

        RowLayout {
            Layout.fillWidth: true

            Text {
                text: "체스 로비"
                font.pixelSize: 28
                font.bold: true
                color: "#333"
            }

            Item { Layout.fillWidth: true }

            Button {
                text: "새로고침"
                onClicked: lobbyBridge.refreshRoomList()
            }

            Button {
                text: "방 만들기"
                highlighted: true
                onClicked: createRoomDialog.open()
            }
        }

        ScrollView {
            Layout.fillWidth: true
            Layout.fillHeight: true
            clip: true

            ListView {
                id: roomListView
                model: lobbyBridge.rooms
                spacing: 10

                delegate: Rectangle {
                    width: roomListView.width
                    height: 70
                    color: "white"
                    radius: 8
                    border.color: "#ddd"

                    RowLayout {
                        anchors.fill: parent
                        anchors.margins: 15
                        spacing: 20

                        ColumnLayout {
                            spacing: 4
                            Text {
                                text: modelData.roomName
                                font.pixelSize: 18
                                font.bold: true
                            }
                            Text {
                                text: "방 번호: #" + modelData.roomId
                                font.pixelSize: 12
                                color: "#666"
                            }
                        }

                        Item { Layout.fillWidth: true }

                        Text {
                            text: modelData.playerCount + " / 4"
                            font.pixelSize: 16
                            color: "#4a90e2"
                        }

                        Button {
                            text: "입장하기"
                            onClicked: lobbyBridge.joinRoom(modelData.roomId)
                        }
                    }
                }
            }
        }
    }

    Dialog {
        id: createRoomDialog
        title: "새 방 만들기"
        standardButtons: Dialog.Ok | Dialog.Cancel
        anchors.centerIn: parent

        Column {
            spacing: 10
            width: 250
            TextField {
                id: roomNameInput
                width: parent.width
                placeholderText: "방 제목을 입력하세요"
            }
        }

        onAccepted: {
            if (roomNameInput.text !== "") {
                lobbyBridge.createRoom(roomNameInput.text)
                roomNameInput.text = ""
            }
        }
    }
}