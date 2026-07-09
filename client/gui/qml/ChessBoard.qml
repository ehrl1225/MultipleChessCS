import QtQuick 2.15
import QtQuick.Controls 2.15

Item {
    id: root
    width: 600; height: 600
    property int cellSize: width / 8

    Grid {
        anchors.fill: parent
        columns: 8
        rows: 8
        Repeater {
            model: 64
            Rectangle {
                width: root.cellSize; height: root.cellSize
                property int row: Math.floor(index / 8)
                property int col: index % 8
                color: (row + col) % 2 === 0 ? "#D18B47" : "#FFCE9E"

                Rectangle {
                    anchors.fill: parent
                    color: "orange"
                    visible: bridge.selectedX === col && bridge.selectedY === row
                    opacity: 0.5
                }

                Rectangle {
                    anchors.centerIn: parent
                    width: parent.width * 0.3; height: width
                    radius: width / 2
                    color: "green"
                    visible: {
                        for (let i=0; i< bridge.possibleMoves.length; i++){
                            if (bridge.possibleMoves[i].x === col && bridge.possibleMoves[i].y === row) return true;
                        }
                        return false;
                    }
                    opacity: 0.4
                }
                MouseArea {
                    anchors.fill: parent
                    onClicked: bridge.cellClicked(parent.col, parent.row)
                }
            }
        }
    }
    Repeater {
        model: bridge.pieces
        delegate: Image {
            id: pieceImg
            width: root.cellSize; height: root.cellSize
            source: modelData.image
            x: modelData.posX * root.cellSize
            y: modelData.posY * root.cellSize

            Behavior on x {
                NumberAnimation { duration: 250; easing.type: Easing.OutCubic }
            }
            Behavior on y {
                NumberAnimation { duration: 250; easing.type: Easing.OutCubic }
            }
        }
    }
}