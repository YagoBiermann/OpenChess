namespace OpenChess.Domain
{
    public struct PlayerDTO(string id, char color, string timeRemaining)
    {
        public string Id { get; } = id;
        public char Color { get; } = color;
        public string TimeRemaining { get; } = timeRemaining;
    }
}