from src.chess.chess_board import ChessBoard
from src.chess.chess_color import ChessColor
from src.chess.chess_piece import ChessPiece

class Chess(object):
    side:ChessColor = ChessColor.White
    selectedChessPiece:ChessPiece
    chessBoard:ChessBoard
    selectedLocation:tuple[int,int]

    def __init__(self, side:ChessColor = ChessColor):
        self.side = side
        self.chessBoard = ChessBoard()

    def setSelectedChessPiece(self, piece:ChessPiece):
        self.selectedChessPiece = piece

    def getSelectedChessPiece(self):
        return self.selectedChessPiece

    def getSelectedChessIndex(self):
        pass

    def getChessBoard(self):
        return self.chessBoard

    @staticmethod
    def indexString(index):
        if index in range(0,8):
            return f"{index+1}WP"
        elif index == 8:
            return "LWR"
        elif index == 9:
            return "RWR"
        elif index == 10:
            return "LWN"
        elif index == 11:
            return "RWN"
        elif index == 12:
            return "LWB"
        elif index == 13:
            return "RWB"
        elif index == 14:
            return "WK"
        elif index == 15:
            return "WQ"
        elif index in range(16,24):
            return f"{index-15}BP"
        elif index == 24:
            return "LBR"
        elif index == 25:
            return "RBR"
        elif index == 26:
            return "LBN"
        elif index == 27:
            return "RBN"
        elif index == 28:
            return "LBB"
        elif index == 29:
            return "RBB"
        elif index == 30:
            return "BK"
        elif index == 31:
            return "BQ"
