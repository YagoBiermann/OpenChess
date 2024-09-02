namespace OpenChess.Domain
{
    internal class Player : IReadOnlyPlayer
    {
        public Guid Id { get; }
        public Color Color { get; }
        public Guid CurrentMatch { get; private set; }
        public bool IsCurrentPlayer { get; set; } = false;
        public TimeSpan TimeRemaining { get; set; }

        public Player(PlayerInfo playerInfo)
        {
            Id = playerInfo.Id;
            Color = playerInfo.Color;
            CurrentMatch = playerInfo.CurrentMatch;
            TimeRemaining = playerInfo.TimeRemaining;
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