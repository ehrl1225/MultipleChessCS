namespace MultipleChessCs.Domain.Chess;


public static class ChessMappingExtensions
{
    public static ChessRoomDto ToDto(this ChessRoom room)
    {
        return new(room._roomId, room.MaxPlayers);
    }
}