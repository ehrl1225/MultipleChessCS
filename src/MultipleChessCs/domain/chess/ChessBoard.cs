namespace Domain.Chess.ChessBoard;
using Domain.Chess.ChessPiece;
using Domain.Chess.ChessLocation;
using Domain.Chess.ChessTeam;
using Domain.Chess.ChessThreat;

class ChessBoard
{
    private ChessPiece?[,] board;
    private ChessPiece[] pieces;

    public ChessBoard()
    {
        board = new ChessPiece?[8, 8];

        pieces = new ChessPiece[32];
        InitChessPieces();
    }

    private void InitChessPieces()
    {
        int x;
        int whiteY = 2;
        int blackY = 7;
        int whiteIndex = 0;
        int blackIndex = 16;
        ChessPiece whiteChessPiece;
        ChessPiece blackChessPiece;

        // y = 2, x = 1 ~ 9, index = 0 ~ 7 white pawn
        // y = 7, x = 1 ~ 9, index = 16 ~ 23 black pawn
        for (x = 1; x < 9; x++)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.PAWN
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY-1, x] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.PAWN
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY, x] = blackChessPiece;
            blackIndex++;
        }

        whiteY = 1;
        blackY = 8;
        // y = 1, x = 1, 8, index = 8, 9 white rook
        // y = 8, x = 1, 8, index = 24, 25 black rook
        for (x = 1; x<9; x += 7)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.ROOK
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY, x] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.ROOK
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY, x] = blackChessPiece;
            blackIndex++;
        }

        // y = 1, x = 2, 7, index = 10, 11 white knight
        // y = 8, x = 2, 7, index = 26, 27 black knight
        for (x= 2; x<8; x += 5)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.KNIGHT
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY, x] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.KNIGHT
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY, x] = blackChessPiece;
            blackIndex++;
        }

        // y = 1, x = 3, 6, index = 12, 13 white bishop
        // y = 8, x = 3, 6, index = 28, 29 black bishop
        for (x= 3; x<7; x += 3)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.BISHOP
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY, x] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.BISHOP
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY, x] = blackChessPiece;
            blackIndex++;
        }

        // y = 1, x = 5, index = 14 white king
        // y = 8, x = 5, index = 30 black king
        x=5;
        whiteChessPiece = new(
            whiteIndex,
            x,
            whiteY,
            ChessTeam.White,
            ChessClass.KING
        );
        pieces[whiteIndex] = whiteChessPiece;
        board[whiteY, x] = whiteChessPiece;
        whiteIndex++;

        blackChessPiece = new(
            blackIndex,
            x,
            blackY,
            ChessTeam.Black,
            ChessClass.KING
        );
        pieces[blackIndex] = blackChessPiece;
        board[blackY, x] = blackChessPiece;
        blackIndex++;

        // y = 1, x = 4, index = 15 white queen
        // y = 8, x = 4, index = 31 black queen
        x=4;
        whiteChessPiece = new(
            whiteIndex,
            x,
            whiteY,
            ChessTeam.White,
            ChessClass.QUEEN
        );
        pieces[whiteIndex] = whiteChessPiece;
        board[whiteY, x] = whiteChessPiece;

        blackChessPiece = new(
            blackIndex,
            x,
            blackY,
            ChessTeam.Black,
            ChessClass.QUEEN
        );
        pieces[blackIndex] = blackChessPiece;
        board[blackY, x] = blackChessPiece;
    }

    private ChessPiece? getByLocation(ChessLocation location)
    {
        ChessPiece? chessPiece = board[location.y-1, location.x-1];
        return chessPiece;
    }

    private bool AddToLocation(
        List<ChessLocation> locations, 
        ChessLocation location,
        ChessTeam team)
    {
        ChessPiece? locationPiece = getByLocation(location);
        if (locationPiece == null)
        {
            locations.Add(location.Copy());
            return false;
        }else if (team != locationPiece.team)
        {
            locations.Add(location.Copy());
            return true;
        }else if (team == locationPiece.team)
        {
            return true;
        }
        return true;
    }

    private void AddRookMovement(ChessPiece chessPiece, List<ChessLocation> locations)
    {
        ChessTeam team = chessPiece.team;
        ChessLocation location = chessPiece.location.Copy();
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                if (dx != 0 && dy != 0) continue;

                location.Move(chessPiece.location);
                for (int i = 0; i < 7; i++)
                {
                    location.AddXY(dx, dy);
                    if (!location.IsInRange()) break;
                    if (AddToLocation(locations, location, team)) break;
                }
            }
        }
    }

    private void AddBishopMovement(ChessPiece chessPiece, List<ChessLocation> locations)
    {
        ChessTeam team = chessPiece.team;
        ChessLocation original_location = chessPiece.location;
        ChessLocation location = chessPiece.location.Copy();
        for (int dx = -1; dx < 2; dx += 2)
        {
            for (int dy = -1; dy < 2; dy += 2)
            {
                location.Move(original_location);
                for (int i = 0; i< 7 ; i++)
                {
                    location.AddXY(dx, dy);
                    if (!location.IsInRange()) break;
                    if (AddToLocation(locations, location, team)) break;
                }
                
            }
        }
    }

    private void AddKnightMovement(ChessPiece chessPiece, List<ChessLocation> locations)
    {
        ChessTeam team = chessPiece.team;
        ChessLocation original_location = chessPiece.location;
        ChessLocation location = chessPiece.location.Copy();

        for (int dx=-1; dx<2; dx+=2)
        {
            for (int dy=-1; dy<2; dy+=2)
            {
                for (int i = 1; i<3; i += 1)
                {
                    location.Move(original_location);
                    location.AddX(i * dx);
                    location.AddY((3-i) * dy);
                    if (location.IsInRange())
                    {
                        AddToLocation(locations, location, team);
                    }
                }
            }
        }
    }

    private void AddPawnMovement(ChessPiece chessPiece, List<ChessLocation> locations)
    {
        int dy = 1;
        if (chessPiece.team == ChessTeam.Black)
        {
            dy = -1;
        }
        ChessLocation originalLocation = chessPiece.location;
        ChessLocation location = originalLocation.Copy();
        if (chessPiece.promoted)
        {

            return;
        }

        location.AddY(dy);
        ChessPiece? locationPiece = getByLocation(location);
        if (locationPiece == null)
        {
            locations.Add(location.Copy());
        }
        if (chessPiece.moveCount == 0)
        {
            location.AddY(dy);
            locationPiece = getByLocation(location);
            if (locationPiece == null)
            {
                locations.Add(location.Copy());
            }
        }
        location.Move(originalLocation);
        location.AddY(dy);
        for (int dx = -1; dx< 2; dx += 2)
        {
            location.MoveX(originalLocation.x + dx);
            locationPiece = getByLocation(location);
            if (locationPiece != null && chessPiece.team != locationPiece.team)
            {
                locations.Add(location.Copy());
            }
        }

        // 앙파상
        location.Move(originalLocation);
        for (int dx = -1; dx<2; dx += 2)
        {
            location.MoveX(originalLocation.x + dx);
            locationPiece = getByLocation(location);
            if (locationPiece != null 
            && chessPiece.team!=locationPiece.team 
            && locationPiece.moveCount == 1)
            {
                location.AddY(dy);
                locations.Add(location.Copy());
                location.MoveY(originalLocation.y);
            }
        }

    }

    private void AddKingMovement(ChessPiece chessPiece, List<ChessLocation> locations)
    {
        ChessTeam team = chessPiece.team;
        ChessLocation originalLocation = chessPiece.location;
        ChessLocation location = chessPiece.location.Copy();
        
        for (int dx = -1; dx<2; dx++)
        {
            location.MoveX(originalLocation.x + dx);
            for (int dy = -1; dy<2; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                location.MoveY(originalLocation.y + dy);
                AddToLocation(locations, location, team);
            }
        }
        // 캐슬링
        if (chessPiece.moveCount == 0)
        {
            location.Move(originalLocation);
            ChessPiece leftRook;
            ChessPiece rightRook;
            if (team == ChessTeam.White)
            {
                leftRook = pieces[8];
                rightRook = pieces[9];
            }else if (team == ChessTeam.Black)
            {
                leftRook = pieces[24];
                rightRook = pieces[25];
            }
            else
            {
                throw new Exception();
            }
            if (leftRook.moveCount == 0)
            {
                location.AddX(-2);
                locations.Add(location.Copy());
                location.Move(originalLocation);
            }
            if (rightRook.moveCount == 0)
            {
                location.AddX(2);
                locations.Add(location.Copy());
                location.Move(originalLocation);
            }
        }
    }

    /**
    매개변수 location을 가리키는 적군을 찾아서 반환합니다.

    */
    private List<ChessThreat> GetThreats(ChessTeam team, ChessLocation location)
    {
        List<ChessThreat> threats = [];

        ChessLocation targetLocation = location.Copy();
        ChessPiece? attackPiece = null;
        List<ChessPiece> allyPieces = [];
        bool blocked = false;
        ChessLocation tmpLocation = location.Copy();
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                if (dx != 0 && dy != 0) continue;
                tmpLocation.Move(targetLocation);
                for (int i = 0; i < 7; i++)
                {
                    tmpLocation.AddXY(dx, dy);
                    if (!tmpLocation.IsInRange()) break;
                    attackPiece = getByLocation(tmpLocation);
                    if (attackPiece == null) continue;
                    if (attackPiece.team == team) allyPieces.Add(attackPiece);
                    else if ((attackPiece.chessClass == ChessClass.ROOK) 
                    || (attackPiece.chessClass == ChessClass.QUEEN))
                    {
                        break;
                    }else if (attackPiece.chessClass == ChessClass.PAWN)
                    {
                        ChessPiece? selfPiece = getByLocation(location);
                        if (attackPiece.promoted 
                        && (attackPiece.promotedClass == ChessClass.ROOK 
                            || attackPiece.promotedClass == ChessClass.QUEEN)
                        )
                        {
                            break;
                        }else if (!attackPiece.promoted
                        && (selfPiece != null)
                        && (dy == 0)
                        && (selfPiece.chessClass == ChessClass.PAWN)
                        && (selfPiece.moveCount == 1))
                        {
                            break;
                        }

                    }

                }
                
                
            }
        }
        for (int x = targetLocation.x - 1; x>0; x--)
        {
            tmpLocation.MoveX(x);
            attackPiece = getByLocation(tmpLocation);
            if (attackPiece == null) continue;
            if (attackPiece.team == team) allyPieces.Add(attackPiece);
            else break;
        }
        if (allyPieces.Count > 0) blocked = true;
        if (attackPiece != null)
        {
            threats.Add(new ChessThreat(
                attackPiece,
                [.. allyPieces],
                blocked,
                targetLocation
            ));
        }
        
        attackPiece = null;
        allyPieces.Clear();
        blocked =false;
        tmpLocation.Move(targetLocation);
        for (int x = targetLocation.x+1; x<9; x++)
        {
            tmpLocation.MoveX(x);
            attackPiece = getByLocation(tmpLocation);
            if (attackPiece == null) continue;
            if (attackPiece.team == team) allyPieces.Add(attackPiece);
            else break;
        }
        if (allyPieces.Count > 0) blocked = true;
        if (attackPiece != null)
        {
            threats.Add(new ChessThreat(
                attackPiece,
                [.. allyPieces],
                blocked,
                targetLocation
            ));
        }
        
        attackPiece = null;
        allyPieces.Clear();
        blocked =false;
        tmpLocation.Move(targetLocation);
        for (int y = targetLocation.y+1; y>0; y--)
        {
            tmpLocation.MoveY(y);
            attackPiece = getByLocation(tmpLocation);
            if (attackPiece == null) continue;
            if (attackPiece.team == team) allyPieces.Add(attackPiece);
            else break;
        }
        if (allyPieces.Count > 0) blocked = true;
        if (attackPiece != null)
        {
            threats.Add(new ChessThreat(
                attackPiece,
                [.. allyPieces],
                blocked,
                targetLocation
            ));
        }

        attackPiece = null;
        allyPieces.Clear();
        blocked =false;
        tmpLocation.Move(targetLocation);
        for (int y = targetLocation.y+1; y<9; y++)
        {
            tmpLocation.MoveY(y);
            attackPiece = getByLocation(tmpLocation);
            if (attackPiece == null) continue;
            if (attackPiece.team == team) allyPieces.Add(attackPiece);
            else break;
        }
        if (allyPieces.Count > 0) blocked = true;
        if (attackPiece != null)
        {
            threats.Add(new ChessThreat(
                attackPiece,
                [.. allyPieces],
                blocked,
                targetLocation
            ));
        }

        attackPiece = null;
        allyPieces.Clear();
        blocked =false;
        tmpLocation.Move(targetLocation);
        for (int i = 0; i<7; i++)
        {
            tmpLocation.AddXY(1, 1);
            if (!tmpLocation.IsInRange())
            {
                break;
            }
            attackPiece = getByLocation(tmpLocation);
            if (attackPiece == null) continue;
            if (attackPiece.team == team) allyPieces.Add(attackPiece);
            else break;
        }
        if (allyPieces.Count > 0) blocked = true;
        if (attackPiece != null)
        {
            threats.Add(new ChessThreat(
                attackPiece,
                [.. allyPieces],
                blocked,
                targetLocation
            ));
        }

        return threats;
    }


    public List<ChessLocation> MoveableLocations(ChessPiece chessPiece)
    {
        List<ChessLocation> locations = [];
        switch (chessPiece.chessClass)
        {
            case ChessClass.ROOK:
                AddRookMovement(chessPiece, locations);
                break;
            case ChessClass.BISHOP:
                AddBishopMovement(chessPiece, locations);
                break;
            case ChessClass.KNIGHT:
                AddKnightMovement(chessPiece, locations);
                break;
            case ChessClass.PAWN:
                AddPawnMovement(chessPiece, locations);
                break;
            case ChessClass.KING:
                AddKingMovement(chessPiece, locations);
                break;
            case ChessClass.QUEEN:
                AddRookMovement(chessPiece, locations);
                AddBishopMovement(chessPiece, locations);
                break;
        }
        return locations;
    }

}
