namespace OpenChess.Application
{
    public readonly struct EndGameDTO(char winner, string status)
    {
        public string Status { get; } = status;
        public char Winner { get; } = winner;
    }
}