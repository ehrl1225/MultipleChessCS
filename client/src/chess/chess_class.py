from enum import Enum

class ChessClass(Enum):
    KING:int = 0
    QUEEN:int = 1
    ROOK:int = 2
    BISHOP:int = 3
    KNIGHT:int = 4
    PAWN:int = 5

    @staticmethod
    def getByNum(num:int):
        if num == 0:
            return ChessClass.KING
        elif num == 1:
            return ChessClass.QUEEN
        elif num == 2:
            return ChessClass.ROOK
        elif num == 3:
            return ChessClass.BISHOP
        elif num == 4:
            return ChessClass.KNIGHT
        elif num == 5:
            return ChessClass.PAWN