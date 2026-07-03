namespace MultipleChessCs.Domain.Chess;


public static class ChessMappingExtensions
{
    public static ChessRoomDto ToDto(this ChessRoom room)
    {
        return new ChessRoomDto(room.RoomId, room.RoomName, room.MaxPlayers, room.GetPlayerCount());
    }

    private static ChessPlayerDto ToDto(this ChessPlayer player)
    {
        return new ChessPlayerDto(player.Username, player.Team);
    }

    public static ChessRoomDetailDto ToDetailDto(this ChessRoom room)
    {
        ChessPlayerDto[] players = new ChessPlayerDto[room.GetPlayerCount()];
        int i = 0;
        foreach (var player in room.GetPlayers())
        {
            players[i] = player.ToDto(); 
            i++;
        }

        return new ChessRoomDetailDto(
            room.RoomId,
            room.RoomName,
            players,
            room.Admin,
            room.IsStarted);
    }
}