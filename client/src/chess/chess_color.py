from enum import Enum

class ChessColor(Enum):
    White:int = 0
    Black:int = 1
    NonColor:int = 2

    def __str__(self):
        if self == ChessColor.White:
            return 'White'
        elif self == ChessColor.Black:
            return 'Black'
        elif self == ChessColor.NonColor:
            return 'NonColor'