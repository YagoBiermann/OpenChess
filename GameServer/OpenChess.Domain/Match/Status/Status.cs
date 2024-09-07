namespace OpenChess.Domain
{
    public class Status(GameStatus gameStatus, GameResult gameResult, CheckStatus checkStatus)
    {
        public GameStatus GameStatus { get; set; } = gameStatus;
        public CheckStatus CheckStatus { get; set; } = checkStatus;
        public GameResult GameResult { get; set; } = gameResult;

        public static explicit operator Status(string value)
        {
            string errorMessage = $"Couldn't parse the string to Status";
            if (string.IsNullOrEmpty(value)) throw new MatchException(errorMessage);
            if (value.Length > 3) throw new MatchException(errorMessage);
            var wasGameStatusParsed = Enum.TryParse(value[0].ToString(), out GameStatus gameStatus);
            var wasGameResultParsed = Enum.TryParse(value[1].ToString(), out GameResult gameResult);
            var wasCheckStatusParsed = Enum.TryParse(value[2].ToString(), out CheckStatus checkStatus);
            if (!wasGameStatusParsed || !wasGameResultParsed || !wasCheckStatusParsed) throw new MatchException(errorMessage);

            return new(gameStatus, gameResult, checkStatus);
        }

        public override string ToString()
        {
            return $"{(int)GameStatus}{(int)GameResult}{(int)CheckStatus}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is Status other)
            {
                return GameStatus == other.GameStatus
                    && GameResult == other.GameResult
                    && CheckStatus == other.CheckStatus;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (GameStatus, GameResult, CheckStatus).GetHashCode();
        }
    }
}