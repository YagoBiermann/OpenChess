namespace OpenChess.Application
{
    public readonly struct JoinMatchDTO(string fen, char color, int time)
    {
        public string Fen { get; } = fen;
        public char Color { get; } = color;
        public int Time { get; } = time;
    }
}