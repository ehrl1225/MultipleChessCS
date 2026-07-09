using MultipleChessCs.Domain.Chess.Enum;

namespace MultipleChessCs.Domain.Chess;

public record ChessRoomDto(
    string roomId,
    string roomName,
    int maxPlayerCount,
    int currentPlayerCount
);

public record ChessPlayerDto(
    string username,
    ChessTeam team,
    bool isHost
);

public record ChessRoomDetailDto(
    string roomId,
    string roomName,
    string admin,
    ChessPlayerDto[] players,
    bool isStarted
);
