namespace OpenChess.Domain
{
    internal class Player
    {
        public Guid Id { get; }
        public Color Color { get; }
        public Guid? CurrentMatch { get; private set; }

        public Player(PlayerInfo info)
        {
            Id = info.Id;
            Color = info.Color;
        }

        public void Join(Guid match)
        {
            if (CurrentMatch is not null) throw new MatchException("Player is already assigned to a match!");
            CurrentMatch = match;
        }

        public void LeaveMatch()
        {
            CurrentMatch = null;
        }
    }
}