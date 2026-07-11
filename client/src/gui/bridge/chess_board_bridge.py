import os
from PyQt6.QtCore import QObject, pyqtSlot, pyqtSignal, QVariant, pyqtProperty
from src.chess.chess_color import ChessColor
from src.chess.chess_class import ChessClass
from src.chess.chess_board import ChessBoard
from src.chess.chess_piece import ChessPiece


class ChessBoardBridge(QObject):
    boardUpdated = pyqtSignal()

    def __init__(self, chess_board: ChessBoard):
        super().__init__()
        self.board = chess_board
        self._selected_x = -1
        self._selected_y = -1

    @pyqtProperty(list, notify=boardUpdated)
    def pieces(self):
        piece_list = []
        for p in self.board.getChessPieces():
            if not p.isDead:
                piece_list.append({
                    "index": p.index,
                    "posX": p.x - 1,
                    "posY": p.y - 1,
                    "image": self._get_image_path(p),
                    "color": "white" if p.chessColor == ChessColor.White else "black"
                })
        return piece_list

    @pyqtProperty(list, notify=boardUpdated)
    def possibleMoves(self):
        if self._selected_x == -1: return []
        piece = self.board.getChessPieceByLocation(self._selected_x, self._selected_y)
        if not piece: return []
        moves = self.board.moveableLocations(piece)
        return [{"x": m.x -1, "y": 8- m.y} for m in moves]

    @pyqtProperty(int, notify=boardUpdated)
    def selectedX(self): return self._selected_y - 1

    @pyqtProperty(int, notify=boardUpdated)
    def selectedY(self): return 8 - self._selected_y

    @pyqtSlot(int, int)
    def cellClicked(self, qmlX, qmlY):
        logicX, logicY = qmlX + 1, 8 - qmlY

        self._selected_x, self._selected_y = logicX, logicY
        self.boardUpdated.emit()

    def _get_image_path(self, p:ChessPiece):
        color_code = "w" if p.chessColor == ChessColor.White else "b"
        class_map = {
            ChessClass.PAWN: 'p', ChessClass.ROOK: 'r', ChessClass.KNIGHT: "n",
            ChessClass.BISHOP: "b", ChessClass.QUEEN: "q", ChessClass.KING: "k",
        }
        cls = p.promotedClass if p.promoted else p.chessClass
        filename = f"{color_code}{class_map[cls]}.png"
        path = os.path.abspath(os.path.join("assets", "pieces", filename))
        return "file:///" + path.replace("\\", "/")
