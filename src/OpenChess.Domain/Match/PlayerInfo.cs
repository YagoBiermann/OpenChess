namespace OpenChess.Domain
{
    internal readonly struct PlayerInfo
    {
        public Guid Id { get; }
        public Color Color { get; }
        public Guid? CurrentMatch { get; }
        public TimeSpan TimeRemaining { get; }

        public PlayerInfo(string id, char color, string currentMatch, long timeRemaining)
        {
            Id = Match.TryParseId(id);
            CurrentMatch = Match.TryParseId(currentMatch);
            Color = ColorUtils.TryParseColor(color);
            TimeRemaining = TimeSpan.FromTicks(timeRemaining);
        }

        public PlayerInfo(Color color, Time durationOfTheMatch)
        {
            Id = Guid.NewGuid();
            Color = color;
            TimeRemaining = TimeSpan.FromMinutes((int)durationOfTheMatch);
        }

        public PlayerInfo(Guid id, Color color, long timeRemaining, Guid? currentMatch = null)
        {
            Id = id;
            Color = color;
            CurrentMatch = currentMatch;
            TimeRemaining = TimeSpan.FromTicks(timeRemaining);
        }
    }
}