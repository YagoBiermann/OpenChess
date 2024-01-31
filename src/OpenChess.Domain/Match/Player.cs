namespace OpenChess.Domain
{
    internal class Player
    {
        public Guid Id { get; }
        public Color Color { get; }
        public Guid CurrentMatch { get; private set; }
        public bool IsCurrentPlayer { get; set; }
        public TimeSpan TimeRemaining { get; set; }

        public Player(PlayerInfo info)
        {
            Id = info.Id;
            Color = info.Color;
            CurrentMatch = info.CurrentMatch;
            IsCurrentPlayer = false;
            TimeRemaining = info.TimeRemaining;
        }

        public PlayerInfo Info
        {
            get { return new(Id, Color, TimeRemaining, CurrentMatch); }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Player player = (Player)obj;
            bool areEqual = player.Color == Color && player.Id == Id && player.CurrentMatch == CurrentMatch && IsCurrentPlayer == player.IsCurrentPlayer;

            return areEqual;
        }

        public override int GetHashCode() => (Id, CurrentMatch, Color, IsCurrentPlayer).GetHashCode();
    }
}