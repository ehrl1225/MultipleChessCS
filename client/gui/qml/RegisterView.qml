import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15

Rectangle {
    id: registerRoot
    color: "#2c3e50"

    Rectangle {
        width: 350
        height: 450
        anchors.centerIn: parent
        radius: 15
        color: "white"

        ColumnLayout {
            anchors.fill: parent
            anchors.margins: 40
            spacing: 20
            Text {
                text: "회원가입"
                font.pixelSize: 24
                font.bold: true
                Layout.alignment: Qt.AlignHCenter
                color: "#2c3e50"
            }

            TextField {
                id: regIdField
                placeholderText: "아이디"
                Layout.fillWidth: true
                font.pixelSize: 14
                color: "black"
                placeholderTextColor: "black"
                background: Rectangle {
                    implicitHeight: 45
                    border.color: regIdField.activeFocus ? "#3498db" : "#bdc3c7"
                    radius: 5
                }
            }

            TextField {
                id: regPwField
                placeholderText: "비밀번호"
                echoMode: TextField.Password
                Layout.fillWidth: true
                font.pixelSize: 14
                color: "black"
                placeholderTextColor: "black"
                background: Rectangle {
                    implicitHeight: 45
                    border.color: regPwField.activeFocus ? "#3498db" : "#bdc3c7"
                    radius: 5
                }
            }

            Button {
                text: "가입하기"
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
                onClicked: authBridge.register(regIdField.text, regPwField.text)
            }

            Text {
                text: "취소"
                font.pixelSize: 12
                color: "black"
                Layout.alignment: Qt.AlignHCenter
                MouseArea {
                    anchors.fill: parent
                    onClicked: {
                        mainStack.pop()
                    }
                }
            }
        }
    }
}