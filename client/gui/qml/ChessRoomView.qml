import QtQuick 2.15
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

        RowLayout {
            ColumnLayout {
                Button {
                    Layout.fillWidth: true
                    text: "관전"
                    font.pixelSize: 30
                    font.bold: true

                    onClicked: {
                        if (roomBridge.isGameStarted) return;
                        roomBridge.joinTeam("Viewer");
                    }
                }
                ScrollView {
                    Layout.fillWidth: true
                    Layout.fillHeight: true
                    clip: true

                    ListView {
                        id: viewerListView
                        model: roomBridge.viewers
                        spacing: 12

                        delegate: Rectangle {
                            width: viewerListView.width
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
                                    color: "grey"
                                    radius: 4
                                    Text {
                                        anchors.centerIn: parent
                                        text: "Viewer"
                                        color: "#333"
                                        font.pixelSize: 12
                                        font.bold: true
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ColumnLayout {
                Button {
                    Layout.fillWidth: true
                    text: "백팀"
                    font.pixelSize: 30
                    font.bold: true

                    onClicked: {
                        if (roomBridge.isGameStarted) return;
                        roomBridge.joinTeam("White")
                    }
                }

                ScrollView {
                    Layout.fillWidth: true
                    Layout.fillHeight: true
                    clip: true

                    ListView {
                        id: whiteTeamListView
                        model: roomBridge.white_teams
                        spacing: 12

                        delegate: Rectangle {
                            width: whiteTeamListView.width
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

                                Item {Layout.fillWidth: true}

                                Rectangle {
                                    width: 60; height: 30
                                    color: "#eee"
                                    radius: 4
                                    Text {
                                        anchors.centerIn: parent
                                        text: "WHITE"
                                        color: "#333"
                                        font.pixelSize: 12
                                        font.bold: true
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ColumnLayout {
                Button {
                    Layout.fillWidth: true
                    text: "흑팀"
                    font.pixelSize: 30
                    font.bold: true

                    onClicked: {
                        if (roomBridge.isGameStarted) return;
                        roomBridge.joinTeam("Black");

                    }
                }
                ScrollView {
                    Layout.fillWidth: true
                    Layout.fillHeight: true
                    clip: true

                    ListView {
                        id: blackTeamListView
                        model: roomBridge.black_teams
                        spacing: 12

                        delegate: Rectangle {
                            width: blackTeamListView.width
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
                                    color: "#333"
                                    radius: 4
                                    Text {
                                        anchors.centerIn: parent
                                        text: "BLACK"
                                        color: "white"
                                        font.pixelSize: 12
                                        font.bold: true
                                    }
                                }
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