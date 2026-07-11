using MultipleChessCs.Domain.Chat;

namespace MultipleChessCs.Common;
using Domain.Chess;


public interface ChessHubInterface
{
    Task RegisterResponse(bool success, string message);
    Task LoginResponse(bool success, string message);
    Task HubResponse(HubAction action, bool success, string message);
    Task GroupNotice(string message);
    Task SendMessage(ChatTarget chatTarget, string sender, string message);
    Task Pong(string message);
    Task Alert(string message);
    Task ChessRoomListResponse(ChessRoomDto[] rooms);
    Task ChessRoomDetailResponse(ChessRoomDetailDto room);
}