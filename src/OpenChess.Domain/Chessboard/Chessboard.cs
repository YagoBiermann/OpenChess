namespace OpenChess.Domain
{
    internal class Chessboard : IReadOnlyChessboard
    {
        private Dictionary<Color, List<IReadOnlyPiece>> _piecesCache { get; } = new();
        private List<List<Square>> _board;
        private PromotionHandler _promotionHandler;
        private EnPassantHandler _enPassantHandler;
        private CastlingHandler _castlingHandler;
        private IMoveHandler _moveHandler;
        private CastlingAvailability _castlingAvailability;
        public ICastlingAvailability CastlingAvailability { get => _castlingAvailability; private set { _castlingAvailability = (CastlingAvailability)value; } }
        private EnPassantAvailability _enPassantAvailability;
        public IEnPassantAvailability EnPassantAvailability { get => _enPassantAvailability; private set { _enPassantAvailability = (EnPassantAvailability)value; } }
        public IMoveCalculator MovesCalculator { get; }
        public Color CurrentPlayer { get; private set; }
        public int HalfMove { get; private set; }
        public int FullMove { get; private set; }
        public string LastPosition { get; private set; }
        public Color Opponent { get => ColorUtils.GetOppositeColor(CurrentPlayer); }

        public Chessboard(string position)
        {
            FenInfo fenPosition = new(position);
            _board = CreateBoard();
            SetPiecesOnBoard(fenPosition.Board);
            CurrentPlayer = fenPosition.ConvertTurn(fenPosition.Turn);
            _castlingAvailability = fenPosition.ConvertCastling(fenPosition.CastlingAvailability);
            Coordinate? enPassantPosition = fenPosition.ConvertEnPassant(fenPosition.EnPassantAvailability);
            _enPassantAvailability = new(enPassantPosition);
            HalfMove = fenPosition.ConvertMoveAmount(fenPosition.HalfMove);
            FullMove = fenPosition.ConvertMoveAmount(fenPosition.FullMove);
            LastPosition = position;
            MovesCalculator = new MovesCalculator(this);
            _enPassantHandler = new(this, MovesCalculator);
            _promotionHandler = new(this, MovesCalculator);
            _castlingHandler = new(this, MovesCalculator);
            _moveHandler = SetupMoveHandlerChain();
        }
        public Piece? AddPiece(Coordinate position, char piece, Color player)
        {
            Piece createdPiece = CreatePiece(piece, position, player);
            Piece? removedPiece = RemovePiece(position);
            GetSquare(position).Piece = createdPiece;
            _piecesCache.Clear();

            return removedPiece;
        }

        public Piece? RemovePiece(Coordinate position)
        {
            Square square = GetSquare(position);
            if (!square.HasPiece) return null;
            Piece piece = square.Piece!;
            square.Piece = null;
            _piecesCache.Clear();

            return piece;
        }
        public List<IReadOnlyPiece> GetAllPieces()
        {
            List<IReadOnlyPiece> allPieces = new();
            _board.ForEach(action: r =>
            {
                var squares = r.FindAll(c => c.HasPiece);
                squares.ForEach(s => allPieces.Add(s.ReadOnlyPiece!));
            });

            return allPieces;
        }
        public IReadOnlySquare GetReadOnlySquare(string coordinate)
        {
            Coordinate origin = Coordinate.GetInstance(coordinate);
            return GetSquare(origin);
        }

        public Square GetSquare(Coordinate coordinate)
        {
            return _board[coordinate.RowToInt][coordinate.ColumnToInt];
        }

        public MovePlayed MovePiece(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (GetPiece(origin) is null) { throw new ChessboardException($"No piece was found in coordinate {origin}!"); }
            IReadOnlyPiece piece = GetPiece(origin)!;
            if (piece.Color != CurrentPlayer) { throw new ChessboardException("It's not your turn"); }
            MovePlayed move = _moveHandler.Handle(piece, destination, promotingPiece);

            HandleIllegalPosition();
            _enPassantAvailability.ClearEnPassant();
            _enPassantAvailability.SetVulnerablePawn(move.PieceMoved, origin);
            _castlingAvailability.UpdateAvailability(origin, CurrentPlayer);
            SwitchTurns();
            _piecesCache.Clear();
            MovesCalculator.CalculateAndCacheAllMoves();

            return move;
        }

        public IReadOnlyPiece? GetPiece(Coordinate position)
        {
            return GetSquare(position).ReadOnlyPiece;
        }

        public List<IReadOnlyPiece> GetPieces(List<Coordinate> range)
        {
            List<IReadOnlyPiece> pieces = new();
            if (!range.Any()) return pieces;
            range.FindAll(c => GetPiece(c) is not null).ToList().ForEach(c =>
            {
                pieces.Add(GetPiece(c)!);
            });

            return pieces;
        }

        public List<IReadOnlyPiece> GetPieces(Color player)
        {
            if (_piecesCache.ContainsKey(player)) { return _piecesCache[player]; }
            List<IReadOnlyPiece> pieces = new();

            _board.ForEach(action: r =>
            {
                var squares = r.FindAll(c => c.HasPiece && c.ReadOnlyPiece?.Color == player);
                squares.ForEach(s => pieces.Add(s.ReadOnlyPiece!));
            });
            _piecesCache.Add(player, pieces);

            return pieces;
        }

        public override string ToString()
        {
            string chessboard = BuildChessboardString();
            string turn = BuildTurnString();
            string castling = BuildCastlingString();
            string enPassant = BuildEnPassantString();

            return $"{chessboard} {turn} {castling} {enPassant} {HalfMove} {FullMove}";
        }

        private void SwitchTurns()
        {
            CurrentPlayer = Opponent;
        }

        private void HandleIllegalPosition()
        {
            CheckHandler checkHandler = new(this, MovesCalculator);
            if (checkHandler.IsInCheck(CurrentPlayer, out CheckState checkAmount)) { RestoreToLastPosition(); throw new ChessboardException("Invalid move!"); }
        }

        private void RestoreToLastPosition()
        {
            Chessboard previous = new(LastPosition);
            _board = previous._board;
            CurrentPlayer = previous.CurrentPlayer;
            _enPassantAvailability = (EnPassantAvailability)previous.EnPassantAvailability;
            _castlingAvailability = (CastlingAvailability)previous.CastlingAvailability;
            HalfMove = previous.HalfMove;
            FullMove = previous.FullMove;
            LastPosition = previous.LastPosition;
        }

        private IMoveHandler SetupMoveHandlerChain()
        {
            _promotionHandler.SetNext(_enPassantHandler);
            _enPassantHandler.SetNext(_castlingHandler);
            _castlingHandler.SetNext(new DefaultMoveHandler(this, MovesCalculator));
            return _promotionHandler;
        }

        private string BuildEnPassantString()
        {
            return _enPassantAvailability.EnPassantPosition is null ? "-" : _enPassantAvailability.EnPassantPosition.ToString();
        }

        private string BuildCastlingString()
        {
            return _castlingAvailability.ToString();
        }

        private string BuildTurnString()
        {
            return CurrentPlayer == Color.Black ? "b" : "w";
        }

        private string BuildChessboardString()
        {
            string chessboard = "";

            for (int row = 7; row >= 0; row--)
            {
                string currentRow = "";
                currentRow += BuildRowString(row);
                chessboard += currentRow;

                if (row == 0) { continue; }
                chessboard += "/";
            }

            return chessboard;
        }

        private string BuildRowString(int row)
        {
            string builtRow = "";
            int amount = 0;
            for (int col = 0; col <= 7; col++)
            {
                Square currentSquare = GetSquare(Coordinate.GetInstance(col, row));
                if (amount > 7) return amount.ToString();
                if (currentSquare.HasPiece)
                {
                    string emptySquares = amount > 0 ? amount.ToString() : string.Empty;
                    builtRow += $"{emptySquares}{currentSquare.ReadOnlyPiece!.Name}";
                    amount = 0;
                    continue;
                }
                amount += 1;
                if (col >= 7) { builtRow += amount; break; }
            }
            return builtRow;
        }

        private static List<List<Square>> CreateBoard()
        {
            List<List<Square>> board = new()
            {
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
                new(),
            };

            for (int row = 0; row <= 7; row++)
            {
                for (int col = 0; col <= 7; col++)
                {
                    board[row].Add(new(Coordinate.GetInstance(col, row), null));
                }
            }

            return board;
        }

        private static Piece CreatePiece(char type, Coordinate origin)
        {
            Color color = char.IsUpper(type) ? Color.White : Color.Black;

            return char.ToUpper(type) switch
            {
                'K' => new King(color, origin),
                'Q' => new Queen(color, origin),
                'R' => new Rook(color, origin),
                'B' => new Bishop(color, origin),
                'N' => new Knight(color, origin),
                'P' => new Pawn(color, origin),
                _ => throw new PieceException($"{type} does not represent a piece"),
            };
        }

        private static Piece CreatePiece(char type, Coordinate origin, Color player)
        {
            char piece = player == Color.Black ? char.ToLower(type) : char.ToUpper(type);
            return CreatePiece(piece, origin);
        }

        private void SetPiecesOnBoard(string field)
        {
            List<string> fenBoard = field.Split("/").Reverse().ToList();

            for (int row = 0; row <= 7; row++)
            {
                int nextPiecePosition = 0;
                for (int col = 0; col <= fenBoard[row].Length - 1; col++)
                {
                    char currentChar = fenBoard[row][col];
                    if (!char.IsDigit(currentChar))
                    {
                        Coordinate origin = Coordinate.GetInstance(nextPiecePosition, row);
                        Piece piece = CreatePiece(currentChar, origin);
                        GetSquare(origin).Piece = piece;

                        nextPiecePosition++;
                        continue;
                    }

                    nextPiecePosition += int.Parse(currentChar.ToString());
                }
            }
        }
    }
}