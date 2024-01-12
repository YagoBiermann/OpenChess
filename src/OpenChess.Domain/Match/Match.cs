namespace OpenChess.Domain
{
    internal class Match
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public int HalfMove { get; private set; }
        public int FullMove { get; private set; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; set; }
        private Stack<string> _pgnMoveText { get; set; }
        private MatchStatus _matchStatus { get; set; }
        private CurrentPositionStatus? _currentPositionStatus { get; set; }
        private TimeSpan _time { get; }
        private DateTime? _currentTurnStartedAt { get; set; }
        private Player? _winner { get; set; }
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
            _currentTurnStartedAt = null;
            _pgnMoveText = new();
            _movesCalculator = new MovesCalculator(_chessboard);
            _currentPositionStatus = Domain.CurrentPositionStatus.NotInCheck;
            HalfMove = FenInfo.ConvertMoveAmount(_fenInfo.HalfMove);
            FullMove = FenInfo.ConvertMoveAmount(_fenInfo.FullMove);
            CreatedAt = DateTime.UtcNow;
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
            var currentTurnStartedAt = matchInfo.CurrentTurnStartedAt;
            var createdAt = matchInfo.CreatedAt;

            Id = matchId;
            _fenInfo = new(fen);
            RestorePlayers(players, matchId);
            SetCurrentPlayer();
            _chessboard = new Chessboard(_fenInfo);
            _movesCalculator = new MovesCalculator(_chessboard);
            _pgnMoveText = pgnMoves;
            _matchStatus = status;
            _time = TimeSpan.FromMinutes((int)time);
            _currentTurnStartedAt = currentTurnStartedAt;
            _currentPositionStatus = null;
            HalfMove = FenInfo.ConvertMoveAmount(_fenInfo.HalfMove);
            FullMove = FenInfo.ConvertMoveAmount(_fenInfo.FullMove);
            CreatedAt = createdAt;

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
            UpdateMoveCounter(movePlayed);
            UpdateEnPassantAndCastlingAvailability(move.Origin, movePlayed.PieceMoved);
            CurrentPositionStatus currentPositionStatus = SetupPositionValidationChain().ValidatePosition();
            ConvertMoveToPGN(movePlayed, currentPositionStatus);
            UpdateMatchStatus(currentPositionStatus);
            _movesCalculator.ClearCache();
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
        public CurrentPositionStatus? CurrentPositionStatus => _currentPositionStatus;
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

        private void ConvertMoveToPGN(MovePlayed movePlayed, CurrentPositionStatus checkState)
        {
            string convertedMove = PGNBuilder.ConvertMoveToPGN(_pgnMoveText.Count, movePlayed, checkState);
            _pgnMoveText.Push(convertedMove);
        }

        private void DeclareWinnerAndFinish()
        {
            _winner = CurrentPlayer;
            _currentPositionStatus = Domain.CurrentPositionStatus.Checkmate;
            _matchStatus = MatchStatus.Finished;
        }

        private void DeclareDrawAndFinish()
        {
            _winner = null;
            _currentPositionStatus = Domain.CurrentPositionStatus.Draw;
            _matchStatus = MatchStatus.Finished;
        }

        private void HandleIllegalPosition()
        {
            CheckValidation checkValidation = new(this, _movesCalculator);
            if (checkValidation.IsInCheck(CurrentPlayerColor!.Value, out CurrentPositionStatus checkAmount)) { RestoreToLastChessboard(); throw new ChessboardException("Invalid move!"); }
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

        private void UpdateMatchStatus(CurrentPositionStatus currentPositionStatus)
        {
            _currentPositionStatus = currentPositionStatus;
            if (currentPositionStatus != Domain.CurrentPositionStatus.Draw && currentPositionStatus != Domain.CurrentPositionStatus.Checkmate) { SwitchTurns(); UpdateFenInfo(); return; }
            if (currentPositionStatus == Domain.CurrentPositionStatus.Checkmate) { UpdateFenInfo(); DeclareWinnerAndFinish(); return; }
            if (currentPositionStatus == Domain.CurrentPositionStatus.Draw) { UpdateFenInfo(); DeclareDrawAndFinish(); return; }
        }

        private void UpdateFenInfo()
        {
            string fenString = FenInfo.BuildFenString(this, CurrentPlayer!);
            _fenInfo = new(fenString);
        }

        private void UpdateMoveCounter(MovePlayed lastMovePlayed)
        {
            bool noCaptureAndNoPawnMoved = lastMovePlayed.PieceCaptured is null && !lastMovePlayed.MoveType.Equals(MoveType.PawnMove);
            if (noCaptureAndNoPawnMoved) HalfMove++; else HalfMove = 0;
            if (CurrentPlayerColor == Color.Black) FullMove++;
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

        private IPositionValidation SetupPositionValidationChain()
        {
            _movesCalculator.CalculateAndCacheAllMoves();
            var moveCounterValidation = new MoveCounterValidation(this);
            var checkValidation = new CheckValidation(this, _movesCalculator);
            var checkmateValidation = new CheckmateValidation(this, _movesCalculator);
            var stalemateValidation = new StalemateValidation(this, _movesCalculator);
            var deadPositionValidation = new DeadPositionValidation(this, _movesCalculator);

            moveCounterValidation.SetNext(checkValidation);
            checkValidation.SetNext(checkmateValidation);
            checkmateValidation.SetNext(stalemateValidation);
            stalemateValidation.SetNext(deadPositionValidation);

            return moveCounterValidation;
        }

    }
}