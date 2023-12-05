namespace OpenChess.Domain
{
    internal class Match
    {
        public Guid Id { get; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; set; }
        private Stack<string> _pgn = new();
        private MatchStatus _status { get; set; }
        private Player? _winner { get; set; }
        private TimeSpan _time { get; }

        public Match(Time time)
        {
            Id = Guid.NewGuid();
            _chessboard = new(FEN.InitialPosition);
            _status = MatchStatus.NotStarted;
            _winner = null;
            _time = TimeSpan.FromMinutes((int)time);
        }

        public void Play(Move move)
        {
            PreValidateMove(move);
            ValidateMove(move);

            MakeMove(move);

            PostValidateMove();
            _chessboard.SwitchTurns();
        }

        public void Join(PlayerInfo playerInfo)
        {
            if (IsFull()) throw new MatchException("Match is full!");

            bool sameColor = GetPlayerByColor(playerInfo.Color) is not null;
            bool sameId = GetPlayerById(playerInfo.Id) is not null;
            if (sameColor) throw new MatchException($"Match already contains a player of same color!");
            if (sameId) throw new MatchException($"Player is already in the match!");

            Guid? currentMatch = playerInfo.CurrentMatch;
            if (currentMatch is not null && currentMatch != Id) { throw new MatchException("Player already assigned to another match!"); }

            Player player = CreateNewPlayer(playerInfo);
            player.Join(Id);
            _players.Add(player);
            if (!IsFull()) { return; };

            _status = MatchStatus.InProgress;
        }

        public bool IsFull()
        {
            return _players.Count == _players.Capacity;
        }

        public bool HasPlayer()
        {
            return _players.Any();
        }

        private void PreValidateMove(Move move)
        {
            if (!HasStarted()) { throw new MatchException("Match did not start yet"); }
            if (HasFinished()) { throw new MatchException("Match already finished"); }
            if (GetPlayerById(move.PlayerId) is null) { throw new MatchException("You are not in this match"); }

            Player player = GetPlayerById(move.PlayerId)!;
            if (!player.Color.Equals(_chessboard.Turn)) { throw new MatchException("Its not your turn!"); };
            if (!_chessboard.GetReadOnlySquare(move.Origin).HasPiece) { throw new ChessboardException("There is no piece in this position"); };

            Color pieceColor = _chessboard.GetReadOnlySquare(move.Origin).ReadOnlyPiece!.Color;
            Color playerColor = GetPlayerById(move.PlayerId)!.Color;
            if (pieceColor != playerColor) { throw new ChessboardException("Cannot move opponent`s piece"); }
        }

        private void ValidateMove(Move move)
        {
            List<MoveDirections> legalMoves = _chessboard.GetReadOnlySquare(move.Origin).ReadOnlyPiece!.CalculateLegalMoves();
            bool cannotMoveToDestination = !legalMoves.Exists(m => m.Coordinates.Contains(move.Destination));
            if (cannotMoveToDestination) { throw new MatchException("Cannot move to given position"); };
        }

        private void PostValidateMove()
        {
            bool isInSelfCheckAfterMove = Check.IsInCheck(_chessboard.Turn, _chessboard);
            if (isInSelfCheckAfterMove) { _chessboard = new Chessboard(_chessboard.LastPosition); throw new MatchException("Invalid move!"); };
        }
        private Player? GetPlayerByColor(Color color)
        {
            return _players.Where(p => p.Color == color).FirstOrDefault();
        }

        private Player? GetPlayerById(Guid id)
        {
            return _players.Where(p => p.Id == id).FirstOrDefault();
        }

        public MatchStatus Status { get { return _status; } }
        public Guid? CurrentPlayer { get { return _players.Find(p => p.Color == _chessboard.Turn)?.Id; } }
        public Time Time { get { return (Time)_time.Minutes; } }
        public Guid? Winner { get { return _winner?.Id; } }
        public Stack<string> Moves { get { return new Stack<string>(_pgn); } }
        public string Chessboard { get { return _chessboard.ToString(); } }

        protected static Player CreateNewPlayer(PlayerInfo info)
        {
            return new Player(info);
        }
    }
}