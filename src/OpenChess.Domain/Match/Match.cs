namespace OpenChess.Domain
{
    internal class Match
    {
        public Guid Id { get; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; set; }
        private Stack<string> _pgnMoveText = new();
        private MatchStatus _status { get; set; }
        private Player? _winner { get; set; }
        private TimeSpan _time { get; }

        public Match(Time time)
        {
            Id = Guid.NewGuid();
            _chessboard = new(FenInfo.InitialPosition);
            _status = MatchStatus.NotStarted;
            _winner = null;
            _time = TimeSpan.FromMinutes((int)time);
        }

        public void Play(Move move)
        {
            ValidateMove(move);
            bool isPawn = _chessboard.GetReadOnlySquare(move.Origin).ReadOnlyPiece is Pawn;

            IReadOnlyPiece? capturedPiece = _chessboard.MovePiece(move.Origin, move.Destination, move.Promoting);
            bool pieceWasCaptured = capturedPiece is not null;

            if (isPawn) BuildPawnPGN(move.Origin, move.Destination, pieceWasCaptured, move.Promoting);
            else BuildDefaultPGN(move.Destination, pieceWasCaptured);
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

        public bool HasStarted()
        {
            return Status.Equals(MatchStatus.InProgress);
        }

        public bool HasFinished()
        {
            return Status.Equals(MatchStatus.Finished);
        }

        public MatchStatus Status { get { return _status; } }
        public Guid? CurrentPlayer
        {
            get
            {
                if (!HasStarted() || HasFinished()) return null;
                return _players.Find(p => p.Color == _chessboard.Turn)?.Id;
            }
        }
        public Time Time { get { return (Time)_time.Minutes; } }
        public Guid? Winner { get { return _winner?.Id; } }
        public Stack<string> Moves { get { return new Stack<string>(_pgnMoveText); } }
        public string Chessboard { get { return _chessboard.ToString(); } }

        protected static Player CreateNewPlayer(PlayerInfo info)
        {
            return new Player(info);
        }

        private void ValidateMove(Move move)
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
            if (move.Promoting is not null && !Promotion.IsValidString(move.Promoting)) { throw new ChessboardException("Invalid promoting piece!"); }
        }

        private void BuildPawnPGN(Coordinate origin, Coordinate destination, bool pieceWasCaptured, string? promotingPiece)
        {
            int moveCount = _pgnMoveText.Count + 1;
            char? parsedPromotionPiece = promotingPiece is not null ? char.Parse(promotingPiece) : null;
            var builder = new PawnTextMoveBuilder(moveCount, origin, destination, parsedPromotionPiece);
            if (pieceWasCaptured) builder.AppendCaptureSign = true;

            builder.Build();
            _pgnMoveText.Push(builder.Result);
        }

        private void BuildDefaultPGN(Coordinate destination, bool pieceWasCaptured)
        {
            int moveCount = _pgnMoveText.Count + 1;
            IReadOnlyPiece movedPiece = _chessboard.GetReadOnlySquare(destination).ReadOnlyPiece!;

            var builder = new DefaultTextMoveBuilder(moveCount, movedPiece, destination);

            if (pieceWasCaptured) { builder.AppendCaptureSign = true; }

            builder.Build();
            _pgnMoveText.Push(builder.Result);
        }

        private Player? GetPlayerByColor(Color color)
        {
            return _players.Where(p => p.Color == color).FirstOrDefault();
        }

        private Player? GetPlayerById(Guid id)
        {
            return _players.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}