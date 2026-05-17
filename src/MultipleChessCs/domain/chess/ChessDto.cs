namespace MultipleChessCs.Domain.Chess;

public record ChessRoomDto(
    string roomId,
    string roomName,
    int maxPlayerCount,
    int currentPlayerCount
);