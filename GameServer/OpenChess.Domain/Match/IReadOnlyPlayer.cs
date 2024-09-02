namespace OpenChess.Domain
{
    public interface IReadOnlyPlayer
    {
        public Guid Id { get; }
        public Color Color { get; }
        public Guid CurrentMatch { get; }
        public bool IsCurrentPlayer { get; }
        public TimeSpan TimeRemaining { get; }
    }
}