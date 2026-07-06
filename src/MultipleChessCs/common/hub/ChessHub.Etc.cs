namespace MultipleChessCs.Common.Hub;

public partial class ChessHub
{
    public async Task Ping(string message)
    {
        Console.WriteLine($"Received : {message}");
        await Clients.Caller.Pong("Pong");
    }
}