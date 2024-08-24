namespace OpenChess.Domain
{
    internal readonly struct PlayerInfo
    {
        public Guid Id { get; }
        public Color Color { get; }
        public Guid CurrentMatch { get; }
        public TimeSpan TimeRemaining { get; }

        public PlayerInfo(string id, char color, string currentMatch, long timeRemaining)
        {
            Id = Match.TryParseId(id);
            CurrentMatch = Match.TryParseId(currentMatch);
            Color = ColorUtils.TryParseColor(color);
            TimeRemaining = TimeSpan.FromTicks(timeRemaining);
        }

        public PlayerInfo(Guid id, Color color, TimeSpan timeRemaining, Guid currentMatch)
        {
            Id = id;
            Color = color;
            CurrentMatch = currentMatch;
            TimeRemaining = TimeSpan.FromTicks(timeRemaining.Ticks);
        }
    }
}