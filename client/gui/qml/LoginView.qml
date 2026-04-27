import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15

Rectangle {
    id: loginRoot
    color: "#2c3e50"

    Rectangle {
        width: 350; height: 450
        anchors.centerIn: parent
        radius: 15
        color: "white"

        ColumnLayout {
            anchors.fill: parent
            anchors.margins: 40
            spacing: 20

            Text {
                text: "CHESS ONLINE"
                font.pixelSize: 28
                font.bold: true
                Layout.alignment: Qt.AlignHCenter
                color: "#2c3e50"
            }

            TextField {
                id: userField
                placeholderText: "아 이 디"
                Layout.fillWidth: true
                font.pixelSize: 14
                background: Rectangle {
                    implicitHeight: 45
                    border.color: userField.activeFocus ? "#3498db" : "#bdc3c7"
                    radius: 5
                }
            }

            TextField {
                id: passField
                placeholderText: "비 밀 번 호"
                echoMode: TextField.Password
                Layout.fillWidth: true
                font.pixelSize: 14
                background: Rectangle {
                    implicitHeight: 45
                    border.color: passField.activeFocus ? "#3498db" : "#bdc3c7"
                    radius: 5
                }
            }

            Text {
                text: LoginViewBridge.errorMessage
                color: "#e74c3c"
                visible: text !== ""
                Layout.alignment: Qt.AlignHCenter
                font.pixelSize: 12
            }

            Button {
                text: "로 그 인"
                Layout.fillWidth: true
                Layout.preferredHeight: 45
                contentItem: Text {
                    text: parent.text
                    color: "white"
                    horizontalAlignment: Text.AlignHCenter
                    verticalAlignment: Text.AlignVCenter
                    font.bold: true
                }
                background: Rectangle {
                    color: parent.pressed ? "#2980b9" : "#3498db"
                    radius: 5
                }
                onClicked: loginViewBridge.login(userField.text, passField.text)
            }

            Text {
                text: "계정이 없으신가요? 회원가입"
                font.pixelSize: 12
                color: "#7f8c8d"
                Layout.alignment: Qt.AlignHCenter
                MouseArea {
                    anchors.fill: parent
                    onClicked: console.log("회원가입 클릭")
                }
            }
        }
    }
}