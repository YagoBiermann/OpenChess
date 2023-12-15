namespace OpenChess.Domain
{
    internal class Match
    {
        public Guid Id { get; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; set; }
        private Stack<string> _pgnMoveText { get; set; }
        private MatchStatus _status { get; set; }
        private TimeSpan _time { get; }
        private Player? _winner { get; set; }

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
            MovePlayed movePlayed = _chessboard.MovePiece(move.Origin, move.Destination, move.Promoting);
            ConvertToPGNMove(movePlayed);
        }

        public void Join(PlayerInfo playerInfo)
        {
            CanJoinMatch(playerInfo, _players, Id);
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
            if (GetPlayerById(move.PlayerId, _players) is null) { throw new MatchException("You are not in this match"); }

            Player player = GetPlayerById(move.PlayerId, _players)!;
            if (!player.Color.Equals(_chessboard.Turn)) { throw new MatchException("Its not your turn!"); };
            if (!_chessboard.GetReadOnlySquare(move.Origin).HasPiece) { throw new ChessboardException("There is no piece in this position"); };

            Color pieceColor = _chessboard.GetReadOnlySquare(move.Origin).ReadOnlyPiece!.Color;
            Color playerColor = GetPlayerById(move.PlayerId, _players)!.Color;
            if (pieceColor != playerColor) { throw new ChessboardException("Cannot move opponent`s piece"); }
        }

        private void ConvertToPGNMove(MovePlayed movePlayed)
        {
            int count = _pgnMoveText.Count + 1;
            bool pieceWasCaptured = movePlayed.PieceCaptured is not null;
            string pgnMove;
            if (movePlayed.MoveType == MoveType.PawnMove || movePlayed.MoveType == MoveType.PawnPromotionMove) pgnMove = PGNBuilder.BuildPawnPGN(count, movePlayed.Origin, movePlayed.Destination, pieceWasCaptured, movePlayed.PromotedPiece);
            else if (movePlayed.MoveType == MoveType.QueenSideCastlingMove) pgnMove = PGNBuilder.BuildQueenSideCastlingString();
            else if (movePlayed.MoveType == MoveType.KingSideCastlingMove) pgnMove = PGNBuilder.BuildKingSideCastlingString();
            else pgnMove = PGNBuilder.BuildDefaultPGN(count, movePlayed.PieceMoved, movePlayed.Destination, pieceWasCaptured);

            _pgnMoveText.Push(pgnMove);
        }

        private static void RestorePlayers(List<Player> players, List<PlayerInfo> playersInfo, Guid matchId)
        {
            foreach (PlayerInfo playerInfo in playersInfo)
            {
                Player restoredPlayer = new(playerInfo);
                if (playerInfo.CurrentMatch is null) throw new MatchException("Player is not in a match!");
                CanJoinMatch(playerInfo, players, matchId);
                players.Add(restoredPlayer);
            }
        }

        private static void CanJoinMatch(PlayerInfo playerInfo, List<Player> players, Guid matchId)
        {
            if (players.Count == 2) throw new MatchException("Match is full!");

            bool sameColor = GetPlayerByColor(playerInfo.Color, players) is not null;
            bool sameId = GetPlayerById(playerInfo.Id, players) is not null;
            if (sameColor) throw new MatchException($"Match already contains a player of same color!");
            if (sameId) throw new MatchException($"Player is already in the match!");

            Guid? currentMatch = playerInfo.CurrentMatch;
            if (currentMatch is not null && currentMatch != matchId) { throw new MatchException("Player already assigned to another match!"); }
        }

        private static Player? GetPlayerByColor(Color color, List<Player> players)
        {
            return players.Where(p => p.Color == color).FirstOrDefault();
        }

        private static Player? GetPlayerById(Guid id, List<Player> players)
        {
            return players.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}