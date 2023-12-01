namespace OpenChess.Domain
{
    internal class Player
    {
        public Guid Id { get; }
        public Color Color { get; }

        public Player(Color color)
        {
            Id = Guid.NewGuid();
            Color = color;
        }
    }
}