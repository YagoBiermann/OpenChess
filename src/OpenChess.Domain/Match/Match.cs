namespace OpenChess.Domain
{
    internal class Match
    {
        public Guid Id { get; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; set; }
        private Stack<string> _pgnMoveText { get; set; }
        private MatchStatus _matchStatus { get; set; }
        private CheckState? _currentPlayerCheckState { get; set; }
        private TimeSpan _time { get; }
        private Player? _winner { get; set; }
        private CheckHandler _checkHandler { get; }

        public Match(Time time)
        {
            Id = Guid.NewGuid();
            _chessboard = new(FenInfo.InitialPosition);
            _matchStatus = MatchStatus.NotStarted;
            _winner = null;
            _time = TimeSpan.FromMinutes((int)time);
            _pgnMoveText = new();
            _checkHandler = new CheckHandler(_chessboard, _chessboard.MovesCalculator);
            _currentPlayerCheckState = CheckState.NotInCheck;
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
            _matchStatus = status;
            _time = TimeSpan.FromMinutes((int)time);
            _checkHandler = new CheckHandler(_chessboard, _chessboard.MovesCalculator);
            _currentPlayerCheckState = null;

            if (winnerId is null) { _winner = null; return; }
            Player winner = GetPlayerById((Guid)winnerId, _players) ?? throw new MatchException("Couldn't determine the winner");
            _winner = winner;
        }

        public void Play(Move move)
        {
            ValidateMove(move);
            MovePlayed movePlayed = _chessboard.MovePiece(move.Origin, move.Destination, move.Promoting);

            bool isInCheckmate = _checkHandler.IsInCheckmate(_chessboard.Turn, out CheckState checkState);
            if (isInCheckmate) DeclareWinnerAndFinish();
            _currentPlayerCheckState = checkState;
            string convertedMove = PGNBuilder.ConvertMoveToPGN(_pgnMoveText.Count, movePlayed, checkState);
            _pgnMoveText.Push(convertedMove);
        }

        public void Join(PlayerInfo playerInfo)
        {
            CanJoinMatch(playerInfo, _players, Id);
            Player player = CreateNewPlayer(playerInfo);
            player.Join(Id);
            _players.Add(player);
            if (!IsFull()) { return; };

            _matchStatus = MatchStatus.InProgress;
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
        public CheckState? CurrentPlayerCheckState { get { return _currentPlayerCheckState; } }
        public MatchStatus Status { get { return _matchStatus; } }
        public PlayerInfo? CurrentPlayer
        {
            get
            {
                if (!HasStarted() || HasFinished()) return null;
                return GetPlayerByColor(_chessboard.Turn);
            }
        }

        public PlayerInfo? OpponentPlayer
        {
            get
            {
                if (!IsFull()) return null;
                return GetPlayerByColor(_chessboard.Opponent);
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
        public IReadOnlyChessboard Chessboard { get { return _chessboard; } }
        public List<PlayerInfo> Players
        {
            get
            {
                List<PlayerInfo> players = new();
                _players.ForEach(p => players.Add(p.Info));

                return players;
            }
        }

        public PlayerInfo GetPlayerByColor(Color color)
        {
            PlayerInfo player = (_players.Find(p => p.Color == color)?.Info) ?? throw new MatchException("player not found");

            return player;
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

        private void DeclareWinnerAndFinish()
        {
            _winner = GetPlayerByColor(_chessboard.Opponent, _players);
            _currentPlayerCheckState = CheckState.Checkmate;
            _matchStatus = MatchStatus.Finished;
        }
    }
}