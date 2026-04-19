namespace Common.ChessManager;
using System.Collections.Concurrent;
using Domain.Chess.ChessRoom;



public class ChessManager
{
    private readonly ConcurrentDictionary<string, ChessRoom> _rooms;
    private readonly int _maxRoomCount;
    private int roomCount;

    public ChessManager(int maxRoomCount = 100)
    {
        _rooms = new ConcurrentDictionary<string, ChessRoom>();
        _maxRoomCount = maxRoomCount;
        roomCount = 0;
    }
    public bool CreateRoom()
    {
        if (roomCount == _maxRoomCount)
        {
            return false;
        }
        string roomId = Guid.NewGuid().ToString("N");
        ChessRoom room = new ChessRoom(roomId);
        if (_rooms.TryAdd(roomId, room))
        {
            return true;
        }
        return false;
    }

    public bool DeleteRoom(string roomId)
    {
        ChessRoom? room;
        bool result = _rooms.TryRemove(roomId, out room);
        return result;
    }

    public ChessRoom? GetByRoomId(string roomId)
    {
        ChessRoom? room;
        _rooms.TryGetValue(roomId, out room);
        return room;
    }

    public ICollection<ChessRoom> GetAllRoom()
    {
        ICollection<ChessRoom> allValues = _rooms.Values;
        return allValues;
    }
}