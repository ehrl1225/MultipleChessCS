import QtQuick 2.15
import QtQuick.Controls 2.15
import QtQuick.Layouts 1.15

Rectangle {
    id: registerRoot
    color: "#2c3e50"

    Rectangle {
        width: 350
        height: 500
        anchors.centerIn: parent
        radius: 15
        color: "white"

        ColumnLayout {
            anchors.fill: parent
            anchors.margins: 40
            spacing: 15
            Text {
                text: "회원가입"
                font.pixelSize: 24
                font.bold: true
                Layout.alignment: Qt.AlignHCenter
            }

            TextField {
                id: regId
                placeholderText: "아이디"
                Layout.fillWidth: true
            }

            TextField {
                id: regPw
                placeholderText: "비밀번호"
                echoMode: TextField.Password
                Layout.fillWidth: true
            }

            Button {
                text: "가입하기"
                Layout.fillWidth: true
                Layout.preferredHeight: 45
                onClicked: authBridge.register(regid.text, regPw.text, regNick.text)
            }

            Button {
                text: "취소"
                flat: true
                Layout.alignment: Qt.AlignHCenter
                onClicked: mainStack.pop()
            }
        }
    }
}