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
        private CheckmateHandler _checkmateHandler { get; }
        private CheckHandler _checkHandler { get; }

        public Match(Time time)
        {
            Id = Guid.NewGuid();
            _chessboard = new(FenInfo.InitialPosition);
            _status = MatchStatus.NotStarted;
            _winner = null;
            _time = TimeSpan.FromMinutes((int)time);
            _pgnMoveText = new();
            _checkmateHandler = new CheckmateHandler(_chessboard);
            _checkHandler = new CheckHandler(_chessboard);
        }

        public Match(MatchInfo matchInfo)
        {
            var matchId = matchInfo.MatchId;
            var players = matchInfo.Players;
            var fen = matchInfo.Fen;
            var pgnMoves = matchInfo.PgnMoves;
            var status = matchInfo.Status;
            var time = matchInfo.Time;
            var winnerId = matchInfo.WinnerId;

            Id = matchId;
            RestorePlayers(_players, players, matchId);
            _chessboard = new Chessboard(fen);
            _pgnMoveText = pgnMoves;
            _status = status;
            _time = TimeSpan.FromMinutes((int)time);
            _checkmateHandler = new CheckmateHandler(_chessboard);
            _checkHandler = new CheckHandler(_chessboard);

            if (winnerId is null) { _winner = null; return; }
            Player winner = GetPlayerById((Guid)winnerId, _players) ?? throw new MatchException("Couldn't determine the winner");
            _winner = winner;
        }

        public void Play(Move move)
        {
            ValidateMove(move);
            MovePlayed movePlayed = _chessboard.MovePiece(move.Origin, move.Destination, move.Promoting);

            bool isInCheck = _checkHandler.IsInCheck(_chessboard.Opponent, out int checkAmount);
            bool isInCheckmate = _checkmateHandler.IsInCheckmate(_chessboard.Opponent, checkAmount);

            CheckCondition checkCondition = GetCheckCondition(isInCheck, isInCheckmate);
            ConvertToPGNMove(movePlayed, checkCondition);
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
        public Stack<string> Moves
        {
            get
            {
                Stack<string> moves = new(_pgnMoveText.Reverse());
                return moves;
            }
        }
        public string Chessboard { get { return _chessboard.ToString(); } }
        public List<PlayerInfo> Players
        {
            get
            {
                List<PlayerInfo> players = new();
                _players.ForEach(p => players.Add(p.Info));

                return players;
            }
        }

        public static Guid TryParseId(string id)
        {
            bool parsedCorrectly = Guid.TryParse(id, out Guid parsedId);
            if (!parsedCorrectly) { throw new MatchException($"given id: {id} is invalid!"); }
            return parsedId;
        }

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

        private void ConvertToPGNMove(MovePlayed movePlayed, CheckCondition checkCondition)
        {
            int count = _pgnMoveText.Count + 1;
            string pgnMove;
            if (movePlayed.MoveType == MoveType.PawnMove || movePlayed.MoveType == MoveType.PawnPromotionMove || movePlayed.MoveType == MoveType.EnPassantMove) pgnMove = PGNBuilder.BuildPawnPGN(count, movePlayed, checkCondition);
            else if (movePlayed.MoveType == MoveType.QueenSideCastlingMove) pgnMove = PGNBuilder.BuildQueenSideCastlingString();
            else if (movePlayed.MoveType == MoveType.KingSideCastlingMove) pgnMove = PGNBuilder.BuildKingSideCastlingString();
            else pgnMove = PGNBuilder.BuildDefaultPGN(count, movePlayed, checkCondition);

            _pgnMoveText.Push(pgnMove);
        }

        private static CheckCondition GetCheckCondition(bool isCheck, bool isCheckmate)
        {
            if (isCheck && !isCheckmate) return CheckCondition.Check;
            if (isCheckmate) return CheckCondition.Checkmate;
            return CheckCondition.Default;
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