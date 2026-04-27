import QtQuick 2.15
import QtQuick.Window 2.15
import QtQuick.Controls

ApplicationWindow {
    id: window
    visible: true
    width: 800
    height: 600
    title: qsTr("MultipleChess")

    StackView {
        id: stackView
        anchors.fill: parent
        initialItem: "LoginView.qml"
    }

    function changeView(viewName) {
        stackView.replace(viewName)
    }
}