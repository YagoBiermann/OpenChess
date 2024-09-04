namespace OpenChess.Application
{
    public readonly struct ResignDTO(char winner, string status)
    {
        public string Status { get; } = status;
        public char Winner { get; } = winner;
    }
}