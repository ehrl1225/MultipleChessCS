namespace MultipleChessCs.Domain.Chess.Rules;
using Board;
using Enum;

public class ChessRules
{
    private bool AddToLocation(
        ChessBoard board,
        List<ChessLocation> locations, 
        ChessLocation location,
        ChessTeam team)
    {
        ChessPiece? locationPiece = board.GetByLocation(location);
        if (locationPiece == null)
        {
            locations.Add(location.Copy());
            return false;
        }
        if (team != locationPiece.team)
        {
            locations.Add(location.Copy());
        }else if (team == locationPiece.team)
        {
            
        }
        return true;
    }
    
    /**
    매개변수 location을 가리키는 적을 찾아서 반환합니다.

    */
    private List<ChessThreat> GetThreats(ChessBoard board, ChessTeam team, ChessLocation location)
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
                    attackPiece = board.GetByLocation(tmpLocation);
                    if (attackPiece == null) continue;
                    if (attackPiece.team == team) allyPieces.Add(attackPiece);
                    else if ((attackPiece.chessClass == ChessClass.ROOK) 
                    || (attackPiece.chessClass == ChessClass.QUEEN))
                    {
                        break;
                    }else if (attackPiece.chessClass == ChessClass.PAWN)
                    {
                        ChessPiece? selfPiece = board.GetByLocation(location);
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
                            && (selfPiece.moveCount == 1)
                        )
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
            attackPiece = board.GetByLocation(tmpLocation);
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
            attackPiece = board.GetByLocation(tmpLocation);
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
            attackPiece = board.GetByLocation(tmpLocation);
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
            attackPiece = board.GetByLocation(tmpLocation);
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
            attackPiece = board.GetByLocation(tmpLocation);
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
    
    private void AddRookMovement(ChessBoard board, ChessPiece chessPiece, List<ChessLocation> locations)
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
                    if (AddToLocation(board, locations, location, team)) break;
                }
            }
        }
    }
    
    private void AddBishopMovement(ChessBoard board, ChessPiece chessPiece, List<ChessLocation> locations)
    {
        ChessTeam team = chessPiece.team;
        ChessLocation originalLocation = chessPiece.location;
        ChessLocation location = chessPiece.location.Copy();
        for (int dx = -1; dx < 2; dx += 2)
        {
            for (int dy = -1; dy < 2; dy += 2)
            {
                location.Move(originalLocation);
                for (int i = 0; i< 7 ; i++)
                {
                    location.AddXY(dx, dy);
                    if (!location.IsInRange()) break;
                    if (AddToLocation(board, locations, location, team)) break;
                }
                
            }
        }
    }

    private void AddKnightMovement(ChessBoard board, ChessPiece chessPiece, List<ChessLocation> locations)
    {
        ChessTeam team = chessPiece.team;
        ChessLocation originalLocation = chessPiece.location;
        ChessLocation location = chessPiece.location.Copy();

        for (int dx=-1; dx<2; dx+=2)
        {
            for (int dy=-1; dy<2; dy+=2)
            {
                for (int i = 1; i<3; i += 1)
                {
                    location.Move(originalLocation);
                    location.AddX(i * dx);
                    location.AddY((3-i) * dy);
                    if (location.IsInRange())
                    {
                        AddToLocation(board, locations, location, team);
                    }
                }
            }
        }
    }

    private void AddPawnMovement(ChessBoard board, ChessPiece chessPiece, List<ChessLocation> locations)
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
        ChessPiece? locationPiece = board.GetByLocation(location);
        if (locationPiece == null)
        {
            locations.Add(location.Copy());
        }
        if (chessPiece.moveCount == 0)
        {
            location.AddY(dy);
            locationPiece = board.GetByLocation(location);
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
            locationPiece = board.GetByLocation(location);
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
            locationPiece = board.GetByLocation(location);
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

    private void AddKingMovement(ChessBoard board, ChessPiece chessPiece, List<ChessLocation> locations)
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
                AddToLocation(board, locations, location, team);
            }
        }
        // 캐슬링
        if (chessPiece.moveCount == 0)
        {
            location.Move(originalLocation);
            ChessPiece? leftRook;
            ChessPiece? rightRook;
            if (team == ChessTeam.White)
            {
                leftRook = board.GetByIndex(8);
                rightRook = board.GetByIndex(9);
            }else if (team == ChessTeam.Black)
            {
                leftRook = board.GetByIndex(24);
                rightRook = board.GetByIndex(25);
            }
            else
            {
                throw new Exception();
            }
            if (leftRook?.moveCount == 0)
            {
                location.AddX(-2);
                locations.Add(location.Copy());
                location.Move(originalLocation);
            }
            if (rightRook?.moveCount == 0)
            {
                location.AddX(2);
                locations.Add(location.Copy());
                location.Move(originalLocation);
            }
        }
    }
    public List<ChessLocation> MoveableLocations(ChessBoard board, ChessPiece chessPiece)
    {
        List<ChessLocation> locations = [];
        switch (chessPiece.chessClass)
        {
            case ChessClass.ROOK:
                AddRookMovement(board, chessPiece, locations);
                break;
            case ChessClass.BISHOP:
                AddBishopMovement(board, chessPiece, locations);
                break;
            case ChessClass.KNIGHT:
                AddKnightMovement(board, chessPiece, locations);
                break;
            case ChessClass.PAWN:
                AddPawnMovement(board, chessPiece, locations);
                break;
            case ChessClass.KING:
                AddKingMovement(board, chessPiece, locations);
                break;
            case ChessClass.QUEEN:
                AddRookMovement(board, chessPiece, locations);
                AddBishopMovement(board, chessPiece, locations);
                break;
        }
        return locations;
    }
}