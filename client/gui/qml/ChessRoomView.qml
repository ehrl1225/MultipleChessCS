경import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15

Rectangle {
    id: chessRoomView
    color: "#f0f2f5"

    ColumnLayout {
        anchors.fill: parent
        anchors.margins: 30
        spacing: 20

        RowLayout {
            Layout.fillWidth: true
            Text {
                text: roomBridge.roomName + " 대기실"
                font.pixelSize: 28
                font.bold: true
                color: "#333"
            }
            Item { Layout.fillWidth: true }
            Rectangle {
                width: 100; height: 35
                color: roomBridge.isGameStarted ? "#ff3d3f" : "#52c41a"
                radius: 17
                Text {
                    anchors.centerIn: parent
                    text: roomBridge.isGameStarted ? "게임 중" : "대기 중"
                    color: "white"
                    font.bold: true
                }
            }
        }

        ScrollView {
            Layout.fillWidth: true
            Layout.fillHeight: true
            clip: true

            ListView {
                id: playerListView
                model: roomBridge.players
                spacing: 12

                delegate: Rectangle {
                    width: playerListView.width
                    height: 70
                    color: "white"
                    radius: 10
                    border.color: "#eee"

                    RowLayout {
                        anchors.fill: parent
                        anchors.margins: 20

                        Text {
                            text: modelData.username
                            font.pixelSize: 18
                            font.bold: true
                        }

                        Text {
                            text: modelData.isHost ? "👑방장" : ""
                            color: "#faa140"
                            font.pixelSize: 14
                        }

                        Item { Layout.fillWidth: true }

                        Rectangle {
                            width: 60; height: 30
                            color: modelData.team === 1 ? "#eee" : "#333"
                            radius: 4
                            Text {
                                anchors.centerIn: parent
                                text: modelData.team === 1 ? "WHITE" : "BLACK"
                                color: modelData.team === 1 ? "#333" : "white"
                                font.pixelSize: 12
                                font.bold: true
                            }
                        }
                    }
                }
            }
        }

        RowLayout {
            Layout.fillWidth: true
            spacing: 15

            Button {
                text: "팀 변경"
                onClicked: roomBridge.changeTeam()
            }

            Button {
                text: "방 나가기"
                onClicked: roomBridge.leaveRoom()
            }

            Item { Layout.fillWidth: true }

            Button {
                Layout.preferredWidth: 150
                highlighted: true
                text: roomBridge.isGameStarted ? "게임 화면으로" : "게임 시작"

                enabled: roomBridge.isGameStarted || (roomBridge.isHost && roomBridge.canStart)

                onClicked: {
                    if (roomBridge.isGameStarted) {
                        roomBridge.goToGame()
                    } else {
                        roomBridge.startGame()
                    }
                }
            }
        }
    }
}