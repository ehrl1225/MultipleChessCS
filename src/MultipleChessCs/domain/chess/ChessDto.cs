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
    ChessTeam? team
);

public record ChessRoomDetailDto(
    string roomId,
    string roomName,
    ChessPlayerDto[] players,
    string admin,
    bool isStarted
);
