namespace OpenChess.Domain
{
    internal readonly struct PlayerInfo
    {
        public Guid Id { get; }
        public Color Color { get; }
        public Guid? CurrentMatch { get; }

        public PlayerInfo(char color)
        {
            Id = Guid.NewGuid();
            Color = ColorUtils.TryParseColor(color);
        }

        public PlayerInfo(string id, char color, string currentMatch)
        {
            Id = Match.TryParseId(id);
            CurrentMatch = Match.TryParseId(currentMatch);
            Color = ColorUtils.TryParseColor(color);
        }

        public PlayerInfo(Color color)
        {
            Id = Guid.NewGuid();
            Color = color;
        }

        public PlayerInfo(Guid id, Color color, Guid? currentMatch = null)
        {
            Id = id;
            Color = color;
            CurrentMatch = currentMatch;
        }
    }
}