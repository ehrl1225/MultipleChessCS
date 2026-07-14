import os

from PyQt6.QtCore import pyqtSignal
from PyQt6.QtGui import QPixmap, QColor
from PyQt6.QtWidgets import QTableWidget, QLabel, QTableWidgetItem
from src.chess.chess_board import ChessBoard
from src.chess.chess_class import ChessClass
from src.chess.chess_color import ChessColor
from src.gui.qcolors import Colors

alphas:list[str] = ["A", "B", "C", "D", "E", "F", "G", "H"]
nums:list[str] = ["8", "7", "6", "5", "4", "3", "2", "1"]

baseUrl = os.getcwd()

class ChessBoardWidget(QTableWidget):
    doubleClicked = pyqtSignal(tuple)

    def __init__(self, chessBoard:ChessBoard):
        super().__init__()
        self.chessBoard = chessBoard
        self.selectedRow = -1
        self.selectedColumn = -1
        self.chessColor = ChessColor.NonColor
        self.initUI()

    def initUI(self):
        self.setColumnCount(8)
        self.setRowCount(8)
        self.setHorizontalHeaderLabels(alphas)
        self.setVerticalHeaderLabels(nums)
        self.initTableColor()
        self.setTableSize()
        self.initBoard()
        self.cellDoubleClicked.connect(self.doubleClickedEvent)
        self.cellClicked.connect(self.clickedChessPiece)

    def initTableColor(self) -> None:
        for row in range(8):
            for col in range(8):
                value = row + col
                tableItem = QTableWidgetItem()
                label = QLabel()
                label.setPixmap(QPixmap())
                self.setItem(row, col, tableItem)
                self.setCellWidget(row, col, label)
                if (value%2 == 0):
                    tableItem.setBackground(Colors.brown.value)
                else:
                    tableItem.setBackground(Colors.yellow.value)
            self.resizeRowToContents(row)

    def clickedChessPiece(self):
        self.setTableColor()
        self.setPossibleLocationColor()
        self.setSelectedPieceColor()

    def setColor(self, row:int, col:int, color:QColor):
        self.item(row, col).setBackground(color)

    def setTableColor(self):
        for row in range(8):
            for col in range(8):
                value = row + col
                tableItem = self.item(row, col)
                if value%2 == 0:
                    tableItem.setBackground(Colors.brown.value)
                else:
                    tableItem.setBackground(Colors.yellow.value)

    def setSelectedPieceColor(self):
        if self.selectedRow != -1 and self.selectedColumn != -1:
            self.item(self.selectedRow, self.selectedColumn).setBackground(Colors.orange.value)
            xy = self.rowColumn2xy(self.selectedRow, self.selectedColumn)
            chessPiece =self.chessBoard.getChessPieceByLocation(xy["x"], xy["y"])
            locations = self.chessBoard.moveableLocations(chessPiece)
            for location in locations:
                rowCol = self.xy2rowColumn(location.x, location.y)
                row = rowCol["row"]
                col = rowCol["col"]
                self.item(row, col).setBackground(Colors.blue.value)

    def setTableSize(self):
        for row in range(8):
            for col in range(8):
                imageUrl = self.getImageUrl(ChessClass.PAWN, ChessColor.White)
                pixmap = self.getImage(imageUrl)
                self.setImage(row, col, pixmap)
        for row in range(8):
            self.resizeRowToContents(row)
        for row in range(8):
            for col in range(8):
                self.removeImage(row, col)

    def getImage(self, url:str):
        pixmap = QPixmap(url)
        return pixmap

    def getImageUrl(self, chessClass:ChessClass, chessColor:ChessColor):
        if chessColor == ChessColor.White:
            if chessClass == ChessClass.PAWN:
                return os.path.join(baseUrl ,"assets/pieces/wp.png")
            elif chessClass == ChessClass.ROOK:
                return os.path.join(baseUrl, "assets/pieces/wr.png")
            elif chessClass == ChessClass.KNIGHT:
                return os.path.join(baseUrl, "assets/pieces/wn.png")
            elif chessClass == ChessClass.BISHOP:
                return os.path.join(baseUrl, "assets/pieces/wb.png")
            elif chessClass == ChessClass.QUEEN:
                return os.path.join(baseUrl,  "assets/pieces/wq.png")
            elif chessClass == ChessClass.KING:
                return os.path.join(baseUrl,  "assets/pieces/wk.png")
            else:
                return ""
        elif chessColor == ChessColor.Black:
            if chessClass == ChessClass.PAWN:
                return os.path.join(baseUrl,  "assets/pieces/bp.png")
            elif chessClass == ChessClass.ROOK:
                return os.path.join(baseUrl, "assets/pieces/br.png")
            elif chessClass == ChessClass.KNIGHT:
                return os.path.join(baseUrl, "assets/pieces/bn.png")
            elif chessClass == ChessClass.BISHOP:
                return os.path.join(baseUrl, "assets/pieces/bb.png")
            elif chessClass == ChessClass.QUEEN:
                return os.path.join(baseUrl, "assets/pieces/bq.png")
            elif chessClass == ChessClass.KING:
                return os.path.join(baseUrl, "assets/pieces/bk.png")
            else:
                return ""
        else:
            return ""

    def clearImages(self):
        for row in range(8):
            for col in range(8):
                self.removeImage(row, col)

    def removeImage(self, row:int, col:int):
        tableItem:QLabel|None = self.cellWidget(row, col)
        if tableItem is not None:
            tableItem.setPixmap(QPixmap())


    def setImage(self, row:int, col:int, pixmap:QPixmap):
        tableItem:QLabel|None = self.cellWidget(row, col)
        if tableItem is not None:
            tableItem.setPixmap(pixmap)

    def initBoard(self):
        # white pawn
        row = 6
        for col in range(8):
            imageUrl = self.getImageUrl(ChessClass.PAWN, ChessColor.White)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 7
        for col in [0,7]:
            imageUrl = self.getImageUrl(ChessClass.ROOK, ChessColor.White)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 7
        for col in [1,6]:
            imageUrl = self.getImageUrl(ChessClass.KNIGHT, ChessColor.White)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 7
        for col in [2,5]:
            imageUrl = self.getImageUrl(ChessClass.BISHOP, ChessColor.White)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 7
        col = 3
        imageUrl = self.getImageUrl(ChessClass.QUEEN, ChessColor.White)
        pixmap = self.getImage(imageUrl)
        self.setImage(row, col, pixmap)

        row = 7
        col = 4
        imageUrl = self.getImageUrl(ChessClass.KING, ChessColor.White)
        pixmap = self.getImage(imageUrl)
        self.setImage(row, col, pixmap)

        # black
        row = 1
        for col in range(8):
            imageUrl = self.getImageUrl(ChessClass.PAWN, ChessColor.Black)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 0
        for col in [0,7]:
            imageUrl = self.getImageUrl(ChessClass.ROOK, ChessColor.Black)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 0
        for col in [1,6]:
            imageUrl = self.getImageUrl(ChessClass.KNIGHT, ChessColor.Black)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 0
        for col in [2,5]:
            imageUrl = self.getImageUrl(ChessClass.BISHOP, ChessColor.Black)
            pixmap = self.getImage(imageUrl)
            self.setImage(row, col, pixmap)

        row = 0
        col = 3
        imageUrl = self.getImageUrl(ChessClass.QUEEN, ChessColor.Black)
        pixmap = self.getImage(imageUrl)
        self.setImage(row, col, pixmap)

        row = 0
        col = 4
        imageUrl = self.getImageUrl(ChessClass.KING, ChessColor.Black)
        pixmap = self.getImage(imageUrl)
        self.setImage(row, col, pixmap)

    def xy2rowColumn(self, x:int, y:int):
        return {
            "row":8-y,
            "col":x-1,
        }

    def rowColumn2xy(self, row:int, col:int):
        return {
            "x":col+1,
            "y":8-row
        }

    def setChessBoard(self, chessBoard:ChessBoard):
        self.clearImages()

        for chessPiece in chessBoard.getChessPieces():
            if not chessPiece.isDead:
                row_col = self.xy2rowColumn(chessPiece.x, chessPiece.y)
                row = row_col["row"]
                col = row_col["col"]
                if chessPiece.chessClass == ChessClass.PAWN:
                    if chessPiece.promoted:
                        imageUrl = self.getImageUrl(chessPiece.promotedClass, chessPiece.chessColor)
                    else:
                        imageUrl = self.getImageUrl(chessPiece.chessClass, chessPiece.chessColor)
                else:
                    imageUrl = self.getImageUrl(chessPiece.chessClass, chessPiece.chessColor)
                pixmap = self.getImage(imageUrl)
                self.setImage(row, col, pixmap)
        self.chessBoard = chessBoard

    def doubleClickedEvent(self, row:int, col:int):
        xy = self.rowColumn2xy(row, col)
        self.doubleClicked.emit((xy['x'], xy['y']))

    def kill(self, index:int):
        self.chessBoard.chessPieces[index].isDead = True
        self.setChessBoard(self.chessBoard)

    def movePiece(self, index:int, x:int, y:int):
        self.chessBoard.moveChessPiece(index, x, y)
        self.selectedRow = -1
        self.selectedColumn = -1
        self.setChessBoard(self.chessBoard)

    def promotePiece(self, index:int, chessClass:ChessClass):
        chessPiece = self.chessBoard.getChessPieceByIndex(index)
        chessPiece.promoted = True
        chessPiece.promotedClass = chessClass
        self.setChessBoard(self.chessBoard)

    def selectPiece(self, index:int):
        if index == -1:
            self.selectedRow = -1
            self.selectedColumn = -1
        else:
            chessPiece = self.chessBoard.getChessPieceByIndex(index)
            rowCol = self.xy2rowColumn(chessPiece.x, chessPiece.y)
            row = rowCol["row"]
            col = rowCol["col"]
            self.selectedRow = row
            self.selectedColumn = col
        self.setTableColor()
        self.setSelectedPieceColor()

    def setPossibleLocationColor(self):
        selectedTableItems = self.selectedItems()
        if len(selectedTableItems) == 0:
            return
        selectedItem = selectedTableItems[0]
        row = selectedItem.row()
        col = selectedItem.column()
        xy = self.rowColumn2xy(row, col)
        x = xy['x']
        y = xy['y']
        chessPiece = self.chessBoard.getChessPieceByLocation(x,y)
        if chessPiece is not None:
            locations = self.chessBoard.moveableLocations(chessPiece)
            for location in locations:
                row_col = self.xy2rowColumn(location.x,location.y)
                row = row_col["row"]
                col = row_col["col"]
                if self.chessColor == chessPiece.chessColor:
                    self.setColor(row, col, Colors.green.value)
                else:
                    self.setColor(row, col, Colors.red.value)
