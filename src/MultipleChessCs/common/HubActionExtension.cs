namespace MultipleChessCs.Common;

public static class HubActionExtension
{
    public static int ToInt(this HubAction action) => (int)action;
}