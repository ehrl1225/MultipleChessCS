namespace Domain.Chess.ChessMappingExtensions;

using Domain.Chess.ChessDto;
using Domain.Chess.ChessRoom;

public static class ChessMappingExtensions
{
    public static ChessRoomDto ToDto(this ChessRoom room)
    {
        return new(room._roomId, room.MaxPlayers);
    }
}