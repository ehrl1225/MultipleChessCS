from typing import Optional

from src.chess.chess_class import ChessClass
from src.chess.chess_color import ChessColor

alpha: list[str] = "A B C D E F G H".split(" ")


class Location:
    x: int
    y: int

    def __init__(self, x: int, y: int) -> None:
        self.x = x
        self.y = y

    def __add__(self, other: 'Location') -> 'Location':
        return Location(self.x + other.x, self.y + other.y)


rookRLocations: list[Location] = [Location(-i, 0) for i in range(1, 8)] + [Location(i, 0) for i in range(1, 8)] + [
    Location(0, -i) for i in range(1, 8)] + [Location(0, i) for i in range(1, 8)]

bishopRLocations: list[Location] = [Location(-i, -i) for i in range(1, 8)] + [Location(-i, +i) for i in range(1, 8)] + [
    Location(i, i) for i in range(1, 8)] +[Location(i, -i) for i in range(1, 8)]

knightRLocations: list[Location] = [Location(-1, 2), Location(1,2), Location(2,1), Location(2,-1), Location(-1,-2), Location(1, -2), Location(-2, 1), Location(-2, -1)]

queenRLocations:list[Location] = [Location(-i, 0) for i in range(1, 8)] + [Location(i, 0) for i in range(1, 8)
   ] + [ Location(0, -i) for i in range(1, 8)] + [Location(0, i) for i in range(1, 8)] + [
    Location(-i, -i) for i in range(1, 8)] + [Location(-i, +i) for i in range(1, 8)] + [
    Location(i, i) for i in range(1, 8)] +[Location(i, -i) for i in range(1, 8)]

kingRLocations:list[Location] = [Location(-1, 0), Location(-1, -1), Location(0,-1), Location(1, -1), Location(1, 0), Location(1, 1), Location(0,1), Location(-1, 1)]



def isInBoard(location: Location) -> bool:
    if 0<location.x<=8 and 0<location.y<=8:
        return True
    else:
        return False

class ChessPiece:
    chessClass: ChessClass = ChessClass.PAWN
    chessColor: ChessColor = ChessColor.White
    index: int = 0
    x: int = 0
    y: int = 0
    isDead: bool = False
    moved: bool = False
    promoted: bool = False
    promotedClass: ChessClass = ChessClass.PAWN

    def locationString(self) -> str:
        return f"{alpha[self.x - 1]}{self.y}"

    def possibleLocations(self) -> Optional[list[Location]]:
        if self.chessClass == ChessClass.PAWN:
            if self.promoted:
                if self.promotedClass == ChessClass.KNIGHT:
                    loc = Location(self.x, self.y)
                    locations: list[Location] = list()
                    for location in rookRLocations:
                        l = loc + location
                        if isInBoard(l):
                            locations.append(l)
                    return locations
            else:
                if self.chessColor == ChessColor.White:
                    pass
                else:
                    pass
        elif self.chessClass == ChessClass.KNIGHT:
            loc = Location(self.x,self.y)
            locations:list[Location] = list()
            for location in knightRLocations:
                l = loc+location
                if isInBoard(l):
                    locations.append(l)
            return locations
        elif self.chessClass == ChessClass.KING:
            loc = Location(self.x,self.y)
            locations:list[Location] = list()
            for location in kingRLocations:
                l = loc+location
                if isInBoard(l):
                    locations.append(l)
            return locations
        return None
