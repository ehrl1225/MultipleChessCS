namespace MultipleChessCs.Common;
using MultipleChessCs.Domain.Chess.Rules;
using System.Collections.Concurrent;
using Domain.Chess;


public class ChessManager(ChessRules chessRules)
{
    private readonly ConcurrentDictionary<string, ChessRoom> _rooms = new();
    private readonly int _maxRoomCount = 100;
    private readonly ChessRules _chessRules = chessRules;
    private readonly ConcurrentDictionary<string, ChessRoom?> _joinedRooms = new();
    
    private int RoomCount => _rooms.Count;
    
    public bool CreateRoom(string admin, string connectionId, string roomName, int maxPlayerCount)
    {
        if (RoomCount == _maxRoomCount) return false;
        
        string roomId = Guid.NewGuid().ToString("N");
        ChessRoom room = new(roomId, roomName, admin, maxPlayerCount, _chessRules);
        if (!room.TryJoin(admin, connectionId)) return false;
        if (!_rooms.TryAdd(roomId, room)) return false;
        if (!_joinedRooms.TryAdd(admin, room)) return false;
        return true;
    }

    public bool StartGame(string roomId, string admin)
    {
        ChessRoom? room = GetByRoomId(roomId);
        if (room == null) return false;
        return room.StartGame(admin);
    }

    public bool DeleteRoom(string roomId)
    {
        ChessRoom? room = GetByRoomId(roomId);
        if (room == null) return false;
        foreach (ChessPlayer player in room.GetPlayers())
        {
            room.KickPlayer(player.Username);
        }
        bool result = _rooms.TryRemove(roomId, out room);
        return result;
    }

    public ChessRoom[] GetChessRooms()
    {
        return _rooms.Values.ToArray();
    }

    public ChessRoom? GetByRoomId(string roomId)
    {
        ChessRoom? room;
        _rooms.TryGetValue(roomId, out room);
        return room;
    }

    public ChessRoom? GetByUsername(string username)
    {
        ChessRoom? room;
        _joinedRooms.TryGetValue(username, out room);
        return room;
    }
}