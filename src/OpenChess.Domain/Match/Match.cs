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
        private CurrentPositionStatus _currentPositionStatus { get; set; }
        private DateTime _currentTurnStartedAt { get; set; }
        private Time _duration { get; }
        private Player? _winner { get; set; }
        private FenInfo _fenInfo { get; set; }
        private IMoveCalculator _movesCalculator;

        public Match(Time time)
        {
            Id = Guid.NewGuid();
            _fenInfo = new(FenInfo.InitialPosition);
            _chessboard = new Chessboard(_fenInfo);
            _winner = null;
            _duration = time;
            _currentTurnStartedAt = DateTime.UtcNow;
            _pgnMoveText = new();
            _movesCalculator = new MovesCalculator(_chessboard);
            _currentPositionStatus = Domain.CurrentPositionStatus.NotInCheck;
            HalfMove = FenInfo.ConvertMoveAmount(_fenInfo.HalfMove);
            FullMove = FenInfo.ConvertMoveAmount(_fenInfo.FullMove);
            CreatedAt = DateTime.UtcNow;

            var whitePlayerInfo = new PlayerInfo(Guid.NewGuid(), Color.White, TimeSpan.FromMinutes((int)time), Id);
            var blackPlayerInfo = new PlayerInfo(Guid.NewGuid(), Color.Black, TimeSpan.FromMinutes((int)time), Id);
            CreatePlayer(whitePlayerInfo);
            CreatePlayer(blackPlayerInfo);
            StartMatch();
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
            foreach (var player in players)
            {
                CreatePlayer(player);
            }
            SetCurrentPlayer();
            _chessboard = new Chessboard(_fenInfo);
            _movesCalculator = new MovesCalculator(_chessboard);
            _pgnMoveText = pgnMoves;
            _matchStatus = status;
            _duration = time;
            _currentTurnStartedAt = currentTurnStartedAt;
            _currentPositionStatus = Domain.CurrentPositionStatus.Undefined;
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
            Clock clock = new(_currentTurnStartedAt, CurrentPlayer!.TimeRemaining.Ticks);
            if (!clock.HasTimeEnough()) { DeclareTimeoutAndFinish(); return; }

            var moveHandlers = SetupMoveHandlerChain();
            IReadOnlyPiece piece = _chessboard.GetPiece(move.Origin) ?? throw new MatchException("Piece not found!");
            MovePlayed movePlayed = moveHandlers.Handle(piece, move.Destination, move.Promoting);
            HandleIllegalPosition();
            UpdateMoveCounter(movePlayed);
            UpdateEnPassantAndCastlingAvailability(move.Origin, movePlayed.PieceMoved);
            CurrentPositionStatus currentPositionStatus = SetupPositionValidationChain().ValidatePosition();
            ConvertMoveToPGN(movePlayed, currentPositionStatus);
            UpdateMatchStatus(currentPositionStatus, clock);
            _movesCalculator.ClearCache();
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
        public Time Duration => _duration;
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


        private void CreatePlayer(PlayerInfo player)
        {
            CanJoinMatch(player);
            _players.Add(new(player));
        }

        private void CanJoinMatch(PlayerInfo playerInfo)
        {
            if (_players.Count == 2) throw new MatchException("Match is full!");

            bool sameColor = GetPlayerByColor(playerInfo.Color) is not null;
            bool sameId = GetPlayerById(playerInfo.Id) is not null;
            if (sameColor) throw new MatchException($"Match already contains a player of same color!");
            if (sameId) throw new MatchException($"Player is already in the match!");

            Guid currentMatch = playerInfo.CurrentMatch;
            if (currentMatch != Id) { throw new MatchException("Player already assigned to another match!"); }
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

        private void StartNewTurn()
        {
            _currentTurnStartedAt = DateTime.UtcNow;
        }

        private void SwitchTurns(Clock clock)
        {
            UpdateTimeRemainingForCurrentPlayer(clock);
            StartNewTurn();
            var currentPlayer = CurrentPlayer;
            var opponentPlayer = OpponentPlayer;
            currentPlayer!.IsCurrentPlayer = false;
            opponentPlayer!.IsCurrentPlayer = true;
        }

        private void StartMatch()
        {
            SetCurrentPlayer();
            _matchStatus = MatchStatus.InProgress;
            StartNewTurn();
        }

        private void SetCurrentPlayer()
        {
            Color currentPlayer = FenInfo.ConvertTurn(_fenInfo.Turn);
            GetPlayerByColor(currentPlayer)!.IsCurrentPlayer = true;
        }

        private void ConvertMoveToPGN(MovePlayed movePlayed, CurrentPositionStatus checkState)
        {
            string convertedMove = PGNBuilder.ConvertMoveToPGN(_pgnMoveText.Count, movePlayed, checkState);
            _pgnMoveText.Push(convertedMove);
        }

        private void DeclareTimeoutAndFinish()
        {
            _winner = OpponentPlayer;
            _currentPositionStatus = Domain.CurrentPositionStatus.Timeout;
            _matchStatus = MatchStatus.Finished;
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

        private void UpdateMatchStatus(CurrentPositionStatus currentPositionStatus, Clock clock)
        {
            _currentPositionStatus = currentPositionStatus;
            if (currentPositionStatus != Domain.CurrentPositionStatus.Draw && currentPositionStatus != Domain.CurrentPositionStatus.Checkmate) { SwitchTurns(clock); UpdateFenInfo(); return; }
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

        private void UpdateTimeRemainingForCurrentPlayer(Clock clock)
        {
            CurrentPlayer!.TimeRemaining = clock.CalculateTimeRemainingForCurrentPlayer();
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