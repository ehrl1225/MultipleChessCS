from src.chess.chess_class import ChessClass
from src.chess.chess_color import ChessColor
from src.chess.chess_piece import ChessPiece, knightRLocations, Location

class ChessBoard(object):
    """

    """
    chessPieces: list[ChessPiece] = [ChessPiece() for _ in range(32)]

    def __init__(self):
        """
        """
        self.chessPieces = [ChessPiece() for _ in range(32)]
        self.initChessPieces()

    def getChessPieceByLocation(self, x, y) -> ChessPiece | None:
        for piece in self.chessPieces:
            if piece.isDead:
                continue
            if piece.x == x and piece.y == y:
                return piece
        return None

    def getChessPieceByIndex(self, index:int):
        return self.chessPieces[index]

    def initChessPieces(self):
        y = 2
        index = 0
        for x in range(1,9):
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.PAWN
            chessPiece.chessColor = ChessColor.White
            chessPiece.promoted = False
            chessPiece.promotedClass = ChessClass.PAWN
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index += 1

        y = 1
        for x in [1,8]:
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.ROOK
            chessPiece.chessColor = ChessColor.White
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index+=1

        for x in [2,7]:
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.KNIGHT
            chessPiece.chessColor = ChessColor.White
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index+=1

        for x in [3,6]:
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.BISHOP
            chessPiece.chessColor = ChessColor.White
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index+=1

        x= 5
        chessPiece = self.chessPieces[index]
        chessPiece.x = x
        chessPiece.y = y
        chessPiece.chessClass = ChessClass.KING
        chessPiece.chessColor = ChessColor.White
        chessPiece.index = index
        chessPiece.moved = False
        chessPiece.isDead = False
        index+=1

        x=4
        chessPiece = self.chessPieces[index]
        chessPiece.x = x
        chessPiece.y = y
        chessPiece.chessClass = ChessClass.QUEEN
        chessPiece.chessColor = ChessColor.White
        chessPiece.index = index
        chessPiece.moved = False
        chessPiece.isDead = False
        index+=1

        y=7
        for x in range(1,9):
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.PAWN
            chessPiece.chessColor = ChessColor.Black
            chessPiece.promoted = False
            chessPiece.promotedClass = ChessClass.PAWN
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index+=1

        y= 8
        for x in [1,8]:
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.ROOK
            chessPiece.chessColor = ChessColor.Black
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index+=1

        for x in [2,7]:
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.KNIGHT
            chessPiece.chessColor = ChessColor.Black
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index+=1

        for x in [3,6]:
            chessPiece = self.chessPieces[index]
            chessPiece.x = x
            chessPiece.y = y
            chessPiece.chessClass = ChessClass.BISHOP
            chessPiece.chessColor = ChessColor.Black
            chessPiece.index = index
            chessPiece.moved = False
            chessPiece.isDead = False
            index+=1

        x= 5
        chessPiece = self.chessPieces[index]
        chessPiece.x = x
        chessPiece.y = y
        chessPiece.chessClass = ChessClass.KING
        chessPiece.chessColor = ChessColor.Black
        chessPiece.index = index
        chessPiece.moved = False
        chessPiece.isDead = False
        index+=1

        x= 4
        chessPiece = self.chessPieces[index]
        chessPiece.x = x
        chessPiece.y = y
        chessPiece.chessClass = ChessClass.QUEEN
        chessPiece.chessColor = ChessColor.Black
        chessPiece.index = index
        chessPiece.moved = False
        chessPiece.isDead = False

    def moveChessPiece(self, chess_piece_index:int, x:int, y:int):
        enemy = self.getChessPieceByLocation(x,y)
        chess_piece = self.chessPieces[chess_piece_index]
        if enemy is not None:
            if chess_piece.chessColor == enemy.chessColor:
                # if {chess_piece.chessClass, enemy.chessClass} == {ChessClass.KING, ChessClass.ROOK}:
                #     king = chess_piece
                #     rook = enemy
                #     if king.chessClass == ChessClass.KING:
                #         king,rook = rook, king
                #     if king.moved == False and rook.moved == False:
                #         if rook.x == 1:
                #             for x in range(2,5):
                #                 chess_piece = self.getChessPieceByLocation(x, rook.y)
                #                 if chess_piece is not None:
                #                     break
                #             else:
                #                 king.x = 3
                #                 rook.x = 4
                #         elif rook.x == 8:
                #             for x in range(6,8):
                #                 chess_piece = self.getChessPieceByLocation(x, rook.y)
                #                 if chess_piece is not None:
                #                     break
                #             else:
                #                 king.x = 7
                #                 rook.x = 6
                return
            else:
                enemy.isDead = True
        chess_piece.x = x
        chess_piece.y = y
        chess_piece.moved = True

    def getChessPieces(self):
        return self.chessPieces.copy()

    def getWhiteKing(self):
        return self.chessPieces[14]

    def getBlackKing(self):
        return self.chessPieces[30]

    def getWhiteTeam(self):
        return self.chessPieces[:16]

    def getBlackTeam(self):
        return self.chessPieces[16:]

    def getEnemyTeam(self, color:ChessColor):
        if color == ChessColor.Black:
            return self.getWhiteTeam()
        elif color == ChessColor.White:
            return self.getBlackTeam()

    def isWhiteCheckMate(self):
        whiteKing = self.getWhiteKing()


    def isBlackCheckMate(self):
        pass

    def getPromotablePieceIndex(self) -> int:
        for i in range(8):
            chessPiece = self.getChessPieceByIndex(i)
            if chessPiece.promoted:
                continue
            if chessPiece.y == 8:
                return i

        for i in range(16,24):
            chessPiece = self.getChessPieceByIndex(i)
            if chessPiece.promoted:
                continue
            if chessPiece.y == 1:
                return i
        return -1


    def isStaleMate(self) -> bool:
        # white
        for chess_piece in self.getWhiteTeam():
            if self.canMove(chess_piece):
                break
        else:
            return True
        # black
        for chess_piece in self.getBlackTeam():
            if self.canMove(chess_piece):
                break
        else:
            return True
        return False


    def isWhiteKingDead(self):
        return self.getWhiteKing().isDead

    def isBlackKingDead(self):
        return self.getBlackKing().isDead

    # 체크메이트인지 스테일메이트인지 확인하기 위한 용도로 도착지점에 어느 말이든 있는지 확인하지 않습니다.
    def canAttackHere(self, chessPiece:ChessPiece, x:int, y:int, ignoreDestination = True) -> bool:

        def canRookAttack() -> bool:
            nonlocal chessPiece
            nonlocal x
            nonlocal y
            if chessPiece.x == x:
                for chp in self.chessPieces:
                    if chp.index == chessPiece.index:
                        continue
                    if chp.isDead:
                        continue
                    if chp.x != x:
                        continue
                    if min(chessPiece.y, y) <chp.y<max(chessPiece.y, y):

                        return False
                else:

                    return True
            elif chessPiece.y == y:
                for chp in self.chessPieces:
                    if chp.index == chessPiece.index:
                        continue
                    if chp.isDead:
                        continue
                    if chp.y != y:
                        continue
                    if min(chessPiece.x, x) < chp.x < max(chessPiece.x, x):

                        return False
                    # if not ignoreDestination and chp.x == max(chessPiece.x, x):
                    #     if chp.chessColor == chessPiece.chessColor:
                    #         return False
                    #     else:
                    #         return True
                else:

                    return True
            else:
                return False

        def canBishopAttack() -> bool:
            nonlocal chessPiece
            nonlocal x
            nonlocal y
            diff_x = x - chessPiece.x
            diff_y = y - chessPiece.y
            if abs(diff_x) != abs(diff_y):
                print(f"{x}, {y} 1")
                return False
            distance = abs(diff_x)
            direction = (diff_x // distance, diff_y // distance)
            for chp in self.chessPieces:
                if chp.index == chessPiece.index:
                    continue
                if chp.isDead:
                    continue
                dx = x - chp.x
                dy = y - chp.y
                if abs(dx) != abs(dy):
                    continue
                d = abs(dx)

                if d == 0:
                    continue
                di = (dx // d, dy // d)
                if direction != di:
                    continue
                if distance < d:
                    continue
                print(f"{x} {y} 2")
                print(f"{chp.chessClass}")
                break
            else:
                return True
            return False

        def canKnightAttack() -> bool:
            nonlocal chessPiece
            nonlocal x
            nonlocal y
            for location in chessPiece.possibleLocations():
                if location.x == x and location.y == y:
                    return True
            else:
                return False

        if chessPiece.isDead:
            return False
        if chessPiece.chessClass == ChessClass.PAWN:
            if chessPiece.promoted:
                if chessPiece.promotedClass == ChessClass.QUEEN:
                    return canRookAttack() or canBishopAttack()
                if chessPiece.promotedClass == ChessClass.KNIGHT:
                    return canKnightAttack()
                if chessPiece.promotedClass == ChessClass.ROOK:
                    return canRookAttack()
                if chessPiece.promotedClass == ChessClass.BISHOP:
                    return canBishopAttack()
            else:
                if chessPiece.x-1 != x and chessPiece.x+1 != x:
                    return False
                if chessPiece.chessColor == ChessColor.White:
                    if chessPiece.y+1 == y:
                        return True
                    else:
                        return False
                else:
                    if chessPiece.y-1 == y:
                        return True
                    else:
                        return False
        elif chessPiece.chessClass == ChessClass.ROOK:
            return canRookAttack()
        elif chessPiece.chessClass == ChessClass.BISHOP:
            return canBishopAttack()

        elif chessPiece.chessClass == ChessClass.KNIGHT:
            return canKnightAttack()

        elif chessPiece.chessClass == ChessClass.QUEEN:
            return canRookAttack() or canBishopAttack()

        elif chessPiece.chessClass == ChessClass.KING:
            distance = max(abs(chessPiece.x - x), abs(chessPiece.y - y))
            if distance == 1:
                if not ignoreDestination:
                    pass
                return True
            else:
                return False

    def moveableLocations(self, chessPiece: ChessPiece) -> list[Location]:
        locations:list[Location] = list()

        def addRookMove():
            nonlocal locations
            nonlocal chessPiece
            up_stop = False
            down_stop = False
            left_stop = False
            right_stop = False
            x = chessPiece.x
            y = chessPiece.y
            for i in range(1,8):
                if not up_stop:
                    chp = self.getChessPieceByLocation(x, y+i)
                    if chp is None:
                        if 1<= y+i <= 8:
                            locations.append(Location(x=x, y=y+i))
                        else:
                            up_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x, y + i))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x,y+i))
                            up_stop = True
                if not down_stop:
                    chp = self.getChessPieceByLocation(x, y-i)
                    if chp is None:
                        if 1<= y-i <= 8:
                            locations.append(Location(x=x, y=y-i))
                        else:
                            down_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x, y-i))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x=x, y=y-i))
                            down_stop = True
                if not left_stop:
                    chp = self.getChessPieceByLocation(x-i, y)
                    if chp is None:
                        if 1<= x-i <= 8:
                            locations.append(Location(x=x-i, y=y))
                        else:
                            left_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x=x-i, y=y))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x=x-i, y=y))
                            left_stop = True
                if not right_stop:
                    chp = self.getChessPieceByLocation(x+i, y)
                    if chp is None:
                        if 1<= x+i <= 8:
                            locations.append(Location(x=x+i, y=y))
                        else:
                            right_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x=x+i, y=y))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x=x+i, y=y))
                            right_stop = True

        def addBishopMove():
            nonlocal locations
            nonlocal chessPiece
            up_left_stop = False
            up_right_stop = False
            down_left_stop = False
            down_right_stop = False
            x= chessPiece.x
            y = chessPiece.y
            for i in range(1,8):
                if not up_left_stop:
                    chp = self.getChessPieceByLocation(x-i, y+i)
                    if chp is None:
                        if 1<= x-i <= 8 and 1<= y+i <= 8:
                            locations.append(Location(x=x-i, y=y+i))
                        else:
                            up_left_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x=x-i, y=y+i))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x=x-i, y=y+i))
                            up_left_stop = True
                if not up_right_stop:
                    chp = self.getChessPieceByLocation(x+i, y+i)
                    if chp is None:
                        if 1<= x+i <= 8 and 1<= y+i <= 8:
                            locations.append(Location(x=x+i, y=y+i))
                        else:
                            up_right_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x=x+i, y=y+i))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x=x+i, y=y+i))
                            up_right_stop = True
                if not down_left_stop:
                    chp = self.getChessPieceByLocation(x-i, y-i)
                    if chp is None:
                        if 1<= x-i <= 8 and 1<= y-i <= 8:
                            locations.append(Location(x=x-i, y=y-i))
                        else:
                            down_left_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x=x-i, y=y-i))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x=x-i, y=y-i))
                            down_left_stop = True
                if not down_right_stop:
                    chp = self.getChessPieceByLocation(x+i, y-i)
                    if chp is None:
                        if 1<= x+i <= 8 and 1 <= y-i <= 8:
                            locations.append(Location(x=x+i, y=y-i))
                        else:
                            down_right_stop = True
                    else:
                        if chp.isDead:
                            locations.append(Location(x=x+i, y=y-i))
                        else:
                            if chp.chessColor != chessPiece.chessColor:
                                locations.append(Location(x=x+i, y=y-i))
                            down_right_stop = True

        def addKnightMove():
            nonlocal locations
            nonlocal chessPiece
            x = chessPiece.x
            y = chessPiece.y
            chp = self.getChessPieceByLocation(x-1, y+2)
            if chp is None:
                if 1<= x-1 <= 8 and 1<=y+2 <= 8:
                    locations.append(Location(x=x-1, y=y+2))
            else:
                if chp.isDead:
                    locations.append(Location(x=x-1, y=y+2))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x-1, y=y+2))

            chp = self.getChessPieceByLocation(x+1, y+2)
            if chp is None:
                if 1<= x+1 <= 8 and 1<=y+2 <= 8:
                    locations.append(Location(x=x+1, y=y+2))
            else:
                if chp.isDead:
                    locations.append(Location(x=x+1, y=y+2))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x+1, y=y+2))

            chp = self.getChessPieceByLocation(x+2, y+1)
            if chp is None:
                if 1<= x+2 <= 8 and 1<=y+1 <= 8:
                    locations.append(Location(x=x+2, y=y+1))
            else:
                if chp.isDead:
                    locations.append(Location(x=x+2, y=y+1))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x+2, y=y+1))
            chp = self.getChessPieceByLocation(x+2, y-1)
            if chp is None:
                if 1<= x+2 <= 8 and 1<=+y-1 <= 8:
                    locations.append(Location(x=x+2, y=y-1))
            else:
                if chp.isDead:
                    locations.append(Location(x=x+2, y=y-1))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x+2, y=y-1))
            chp = self.getChessPieceByLocation(x+1, y-2)
            if chp is None:
                if 1<= x+1 <= 8 and 1<=y-2 <= 8:
                    locations.append(Location(x=x+1, y=y-2))
            else:
                if chp.isDead:
                    locations.append(Location(x=x+1, y=y-2))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x+1, y=y-2))

            chp = self.getChessPieceByLocation(x-1, y-2)
            if chp is None:
                if 1<= x-1 <= 8 and 1<=y-2 <= 8:
                    locations.append(Location(x=x-1, y=y-2))
            else:
                if chp.isDead:
                    locations.append(Location(x=x-1, y=y-2))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x-1, y=y-2))

            chp = self.getChessPieceByLocation(x-2, y-1)
            if chp is None:
                if 1<= x-2 <= 8 and 1<=y-1 <= 8:
                    locations.append(Location(x=x-2, y=y-1))
            else:
                if chp.isDead:
                    locations.append(Location(x=x-2, y=y-1))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x-2, y=y-1))

            chp = self.getChessPieceByLocation(x-2, y+1)
            if chp is None:
                if 1<= x-2 <= 8 and 1<=y+1 <= 8:
                    locations.append(Location(x=x-2, y=y+1))
            else:
                if chp.isDead:
                    locations.append(Location(x=x-2, y=y+1))
                else:
                    if chp.chessColor != chessPiece.chessColor:
                        locations.append(Location(x=x-2, y=y+1))

        if chessPiece.chessClass == ChessClass.PAWN:
            if chessPiece.promoted:
                if chessPiece.promotedClass == ChessClass.KNIGHT:
                    addKnightMove()
                elif chessPiece.promotedClass == ChessClass.ROOK:
                    addKnightMove()
                elif chessPiece.promotedClass == ChessClass.BISHOP:
                    addKnightMove()
                elif chessPiece.promotedClass == ChessClass.QUEEN:
                    addRookMove()
                    addBishopMove()

            else:
                if chessPiece.chessColor == ChessColor.White:
                    chp = self.getChessPieceByLocation(chessPiece.x, chessPiece.y+1)
                    if chp is None or chp.isDead:
                        locations.append(Location(x=chessPiece.x, y=chessPiece.y+1))
                        if chessPiece.y == 2:
                            chp = self.getChessPieceByLocation(chessPiece.x, chessPiece.y+2)
                            if chp is None or chp.isDead:
                                locations.append(Location(x=chessPiece.x, y=chessPiece.y+2))
                    chp = self.getChessPieceByLocation(chessPiece.x-1, chessPiece.y+1)
                    if chp is not None and not chp.isDead:
                        if chp.chessColor == ChessColor.Black:
                            locations.append(Location(x=chessPiece.x-1, y=chessPiece.y+1))
                    chp = self.getChessPieceByLocation(chessPiece.x+1, chessPiece.y+1)
                    if chp is not None and not chp.isDead:
                        if chp.chessColor == ChessColor.Black:
                            locations.append(Location(x=chessPiece.x+1, y=chessPiece.y+1))
                elif chessPiece.chessColor == ChessColor.Black:
                    chp = self.getChessPieceByLocation(chessPiece.x, chessPiece.y-1)
                    if chp is None or chp.isDead:
                        locations.append(Location(x=chessPiece.x, y=chessPiece.y-1))
                        if chessPiece.y == 7:
                            chp = self.getChessPieceByLocation(chessPiece.x, chessPiece.y-2)
                            if chp is None or chp.isDead:
                                locations.append(Location(x=chessPiece.x, y=chessPiece.y-2))
                    chp = self.getChessPieceByLocation(chessPiece.x-1, chessPiece.y-1)
                    if chp is not None and not chp.isDead:
                        if chp.chessColor == ChessColor.White:
                            locations.append(Location(x=chessPiece.x-1, y=chessPiece.y-1))
                    chp = self.getChessPieceByLocation(chessPiece.x+1, chessPiece.y-1)
                    if chp is not None and not chp.isDead:
                        if chp.chessColor == ChessColor.White:
                            locations.append(Location(x=chessPiece.x+1, y=chessPiece.y-1))

        elif chessPiece.chessClass == ChessClass.KNIGHT:
            addKnightMove()
        elif chessPiece.chessClass == ChessClass.ROOK:
            addRookMove()
        elif chessPiece.chessClass == ChessClass.BISHOP:
            addBishopMove()
        elif chessPiece.chessClass == ChessClass.QUEEN:
            addRookMove()
            addBishopMove()
        elif chessPiece.chessClass == ChessClass.KING:
            possibleLocations:list[Location] = list()
            x = chessPiece.x
            y = chessPiece.y
            if 1<=x<=8 and 1<=y-1<=8:
                chp = self.getChessPieceByLocation(x,y-1)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x,y=y-1))
                elif chp.chessColor != chessPiece.chessColor:
                    possibleLocations.append(Location(x=x, y=y - 1))

            if 1<=x<=8 and 1<=y+1<=8:
                chp = self.getChessPieceByLocation(x,y+1)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x,y=y+1))
                elif chp.chessColor != chessPiece.chessColor:
                    possibleLocations.append(Location(x=x,y=y+1))

            if 1<=x-1<=8 and 1<=y<=8:
                chp = self.getChessPieceByLocation(x-1, y)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x-1, y=y))
                elif chp.chessColor != chessPiece.chessColor:
                    possibleLocations.append(Location(x=x-1, y=y))

            if 1<=x+1<=8 and 1<=y<=8:
                chp = self.getChessPieceByLocation(x+1, y)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x+1, y=y))
                elif chp.chessColor != chessPiece.chessColor:
                    possibleLocations.append(Location(x=x+1, y=y))

            if 1<=x+1<=8 and 1<= y+1<=8:
                chp = self.getChessPieceByLocation(x+1, y+1)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x+1, y=y+1))

            if 1<=x+1<=8 and 1<=y-1<=8:
                chp = self.getChessPieceByLocation(x+1, y-1)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x+1, y=y-1))

            if 1<=x-1<=8 and 1<=y-1<=8:
                chp = self.getChessPieceByLocation(x-1, y-1)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x-1, y=y-1))
            if 1<=x-1<=8 and 1<=y+1<=8:
                chp = self.getChessPieceByLocation(x-1, y+1)
                if chp is None or chp.isDead:
                    possibleLocations.append(Location(x=x-1, y=y+1))

            for location in possibleLocations:
                enemies = self.getEnemyTeam(chessPiece.chessColor)
                for enemy in enemies:
                    if self.canAttackHere(enemy, location.x, location.y):
                        break
                else:
                    locations.append(location)
        return locations

    def canMove(self, chessPiece: ChessPiece) -> bool:
        if chessPiece.isDead:
            return False
        locations = self.moveableLocations(chessPiece)
        return len(locations) > 0