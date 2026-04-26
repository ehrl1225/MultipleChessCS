namespace MultipleChessCs.Common;
using MultipleChessCs.Domain.Chess.Rules;
using System.Collections.Concurrent;
using Domain.Chess;


public class ChessManager
{
    private readonly ConcurrentDictionary<string, ChessRoom> _rooms;
    private readonly int _maxRoomCount = 100;
    private int roomCount;
    private readonly ChessRules _chessRules;

    public ChessManager(ChessRules chessRules)
    {
        _rooms = new ConcurrentDictionary<string, ChessRoom>();
        _chessRules = chessRules;
        roomCount = 0;
    }
    public bool CreateRoom(string admin, int maxPlayerCount)
    {
        if (roomCount == _maxRoomCount)
        {
            return false;
        }
        string roomId = Guid.NewGuid().ToString("N");
        ChessRoom room = new(roomId, admin, maxPlayerCount, _chessRules);
        if (_rooms.TryAdd(roomId, room))
        {
            return true;
        }
        return false;
    }

    public bool StartGame(string roomId, string admin)
    {
        ChessRoom? room;
        _rooms.TryGetValue(roomId, out room);
        if (room == null) return false;
        return room.StartGame(admin);
    }

    public bool DeleteRoom(string roomId, string admin)
    {
        ChessRoom? room;
        _rooms.TryGetValue(roomId, out room);
        if (room == null) return false;
        if (!room.IsAdmin(admin)) return false;
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
}