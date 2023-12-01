namespace OpenChess.Domain
{
    internal readonly struct PlayerInfo
    {
        public Guid Id { get; }
        public Color Color { get; }

        public PlayerInfo(int color)
        {
            Id = Guid.NewGuid();
            Color = TryParseColor(color);
        }

        public PlayerInfo(string id, int color)
        {
            Id = TryParseId(id);
            Color = TryParseColor(color);
        }

        public PlayerInfo(Color color)
        {
            Id = Guid.NewGuid();
            Color = color;
        }

        public PlayerInfo(Guid id, Color color)
        {
            Id = id;
            Color = color;
        }

        private Color TryParseColor(int color)
        {
            bool colorExists = Enum.IsDefined(typeof(Color), color);
            if (!colorExists) throw new MatchException($"Could not cast the value {color} to a color.");
            return (Color)color;
        }

        private Guid TryParseId(string id)
        {
            bool parsedCorrectly = Guid.TryParse(id, out Guid parsedId);
            if (!parsedCorrectly) { throw new MatchException($"given id: {id} is invalid!"); }
            return parsedId;
        }
    }
}