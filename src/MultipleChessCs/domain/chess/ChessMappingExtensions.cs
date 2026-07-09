namespace MultipleChessCs.Domain.Chess;


public static class ChessMappingExtensions
{
    public static ChessRoomDto ToDto(this ChessRoom room)
    {
        return new ChessRoomDto(room.RoomId, room.RoomName, room.MaxPlayers, room.GetPlayerCount());
    }

    private static ChessPlayerDto ToDto(this ChessPlayer player, bool isHost = false)
    {
        return new ChessPlayerDto(player.Username, player.Team, isHost);
    }

    public static ChessRoomDetailDto ToDetailDto(this ChessRoom room)
    {
        var players = new ChessPlayerDto[room.GetPlayerCount()];
        int i = 0;
        foreach (ChessPlayer player in room.GetPlayers())
        {
            players[i] = player.ToDto(isHost: room.Admin == player.Username);
            i++;
        }

        return new ChessRoomDetailDto(
            room.RoomId,
            room.RoomName,
            room.Admin,
            players,
            room.IsStarted);
    }
}