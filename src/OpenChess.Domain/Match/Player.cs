namespace OpenChess.Domain
{
    internal class Player
    {
        public Guid Id { get; }
        public Color Color { get; }

        public Player(PlayerInfo info)
        {
            Id = info.Id;
            Color = info.Color;
        }
        }
    }
}