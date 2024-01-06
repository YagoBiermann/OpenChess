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
        private FenInfo _fenInfo { get; set; }
        private IMoveCalculator _movesCalculator;

        public Match(Time time)
        {
            Id = Guid.NewGuid();
            _fenInfo = new(FenInfo.InitialPosition);
            _chessboard = new Chessboard(_fenInfo);
            _matchStatus = MatchStatus.NotStarted;
            _winner = null;
            _time = TimeSpan.FromMinutes((int)time);
            _pgnMoveText = new();
            _movesCalculator = new MovesCalculator(_chessboard);
            _checkHandler = new CheckHandler(_chessboard, _movesCalculator);
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
            _fenInfo = new(fen);
            RestorePlayers(players, matchId);
            SetCurrentPlayer();
            _chessboard = new Chessboard(_fenInfo);
            _movesCalculator = new MovesCalculator(_chessboard);
            _checkHandler = new CheckHandler(_chessboard, _movesCalculator);
            _pgnMoveText = pgnMoves;
            _matchStatus = status;
            _time = TimeSpan.FromMinutes((int)time);
            _currentPlayerCheckState = null;

            if (winnerId is null) { _winner = null; return; }
            Player winner = GetPlayerById((Guid)winnerId) ?? throw new MatchException("Couldn't determine the winner");
            _winner = winner;
        }

        public void Play(Move move)
        {
            ValidateMove(move);
            var moveHandlers = SetupMoveHandlerChain();
            IReadOnlyPiece piece = _chessboard.GetPiece(move.Origin) ?? throw new MatchException("Piece not found!");
            MovePlayed movePlayed = moveHandlers.Handle(piece, move.Destination, move.Promoting);
            HandleIllegalPosition();
            UpdateEnPassantAndCastlingAvailability(move.Origin, movePlayed.PieceMoved);

            _movesCalculator.CalculateAndCacheAllMoves();
            bool isInCheckmate = _checkHandler.IsInCheckmate(OpponentPlayerInfo!.Value.Color, out CheckState checkState);
            _currentPlayerCheckState = checkState;
            ConvertMoveToPGN(movePlayed, checkState);

            if (isInCheckmate) { UpdateFenInfo(); DeclareWinnerAndFinish(); return; }
            SwitchTurns();
            UpdateFenInfo();
        }

        public void Join(PlayerInfo playerInfo)
        {
            CanJoinMatch(playerInfo, Id);
            Player player = CreateNewPlayer(playerInfo);
            player.Join(Id);
            _players.Add(player);
            if (!IsFull()) { return; };
            SetCurrentPlayer();
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

        public bool HasStarted() => Status.Equals(MatchStatus.InProgress);
        public bool HasFinished() => Status.Equals(MatchStatus.Finished);
        public string FenString => _fenInfo.Position;
        public CheckState? CurrentPlayerCheckState => _currentPlayerCheckState;
        public MatchStatus Status => _matchStatus;
        public PlayerInfo? CurrentPlayerInfo => CurrentPlayer?.Info;
        public PlayerInfo? OpponentPlayerInfo => OpponentPlayer?.Info;
        public Color? CurrentPlayerColor => CurrentPlayer?.Color;
        public Color? OpponentPlayerColor => OpponentPlayer?.Color;
        public Time Time => (Time)_time.Minutes;
        public Guid? Winner => _winner?.Id;
        public Stack<string> Moves => new(_pgnMoveText.Reverse());
        public IReadOnlyChessboard Chessboard => _chessboard;
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
            Player? player = GetPlayerById(move.PlayerId) ?? throw new MatchException("You are not in this match");
            if (!player.IsCurrentPlayer) throw new MatchException("Its not your turn!");
            if (_chessboard.GetPiece(move.Origin) is null) { throw new ChessboardException("There is no piece in this position"); };

            Color pieceColor = _chessboard.GetPiece(move.Origin)!.Color;
            Color playerColor = GetPlayerById(move.PlayerId)!.Color;
            if (pieceColor != playerColor) { throw new ChessboardException("Cannot move opponent`s piece"); }
        }

        private void RestorePlayers(List<PlayerInfo> playersInfo, Guid matchId)
        {
            foreach (PlayerInfo playerInfo in playersInfo)
            {
                Player restoredPlayer = new(playerInfo);
                if (playerInfo.CurrentMatch is null) throw new MatchException("Player is not in a match!");
                CanJoinMatch(playerInfo, matchId);
                _players.Add(restoredPlayer);
            }
        }

        private void CanJoinMatch(PlayerInfo playerInfo, Guid matchId)
        {
            if (_players.Count == 2) throw new MatchException("Match is full!");

            bool sameColor = GetPlayerByColor(playerInfo.Color) is not null;
            bool sameId = GetPlayerById(playerInfo.Id) is not null;
            if (sameColor) throw new MatchException($"Match already contains a player of same color!");
            if (sameId) throw new MatchException($"Player is already in the match!");

            Guid? currentMatch = playerInfo.CurrentMatch;
            if (currentMatch is not null && currentMatch != matchId) { throw new MatchException("Player already assigned to another match!"); }
        }

        private Player? GetPlayerByColor(Color color)
        {
            return _players.Find(p => p.Color == color);
        }

        private Player? GetPlayerById(Guid id)
        {
            return _players.Find(p => p.Id == id);
        }

        private Player? CurrentPlayer
        {
            get
            {
                if (!HasStarted() || HasFinished()) return null;
                return _players.Find(p => p.IsCurrentPlayer);
            }
        }

        private Player? OpponentPlayer
        {
            get
            {
                if (!HasStarted() || HasFinished()) return null;
                return _players.Find(p => !p.IsCurrentPlayer);
            }
        }

        private void SwitchTurns()
        {
            var currentPlayer = CurrentPlayer;
            var opponentPlayer = OpponentPlayer;
            currentPlayer!.IsCurrentPlayer = false;
            opponentPlayer!.IsCurrentPlayer = true;
        }

        private void SetCurrentPlayer()
        {
            Color currentPlayer = FenInfo.ConvertTurn(_fenInfo.Turn);
            if (IsFull()) { GetPlayerByColor(currentPlayer)!.IsCurrentPlayer = true; };
        }

        private void ConvertMoveToPGN(MovePlayed movePlayed, CheckState checkState)
        {
            string convertedMove = PGNBuilder.ConvertMoveToPGN(_pgnMoveText.Count, movePlayed, checkState);
            _pgnMoveText.Push(convertedMove);
        }

        private void DeclareWinnerAndFinish()
        {
            _winner = CurrentPlayer;
            _currentPlayerCheckState = CheckState.Checkmate;
            _matchStatus = MatchStatus.Finished;
        }

        private void DeclareDrawAndFinish()
        {
            _winner = null;
            _currentPlayerCheckState = CheckState.Draw;
            _matchStatus = MatchStatus.Finished;
        }

        private void HandleIllegalPosition()
        {
            if (_checkHandler.IsInCheck(CurrentPlayerColor!.Value, out CheckState checkAmount)) { RestoreToLastChessboard(); throw new ChessboardException("Invalid move!"); }
        }

        private void RestoreToLastChessboard()
        {
            Chessboard previousChessboard = new(new FenInfo(_fenInfo.Position));
            _chessboard = previousChessboard;
        }
        private void UpdateEnPassantAndCastlingAvailability(Coordinate origin, IReadOnlyPiece pieceMoved)
        {
            _chessboard.EnPassantAvailability.ClearEnPassant();
            _chessboard.EnPassantAvailability.SetVulnerablePawn(pieceMoved, origin);
            _chessboard.CastlingAvailability.UpdateAvailability(origin, CurrentPlayer!.Color);
        }

        private void UpdateFenInfo()
        {
            string fenString = FenInfo.BuildFenString(_chessboard, CurrentPlayer!);
            _fenInfo = new(fenString);
        }

        private IMoveHandler SetupMoveHandlerChain()
        {
            var enPassantHandler = new EnPassantHandler(this, _chessboard, _movesCalculator);
            var promotionHandler = new PromotionHandler(this, _chessboard, _movesCalculator);
            var castlingHandler = new CastlingHandler(this, _chessboard, _movesCalculator);

            promotionHandler.SetNext(enPassantHandler);
            enPassantHandler.SetNext(castlingHandler);
            castlingHandler.SetNext(new DefaultMoveHandler(this, _chessboard, _movesCalculator));
            return promotionHandler;
        }

    }
}