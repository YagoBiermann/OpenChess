namespace OpenChess.Domain
{
    internal class Chessboard : IReadOnlyChessboard
    {
        private List<List<Square>> _board;
        private PromotionHandler _promotionHandler;
        private EnPassantHandler _enPassantHandler;
        private CastlingHandler _castlingHandler;
        private IMoveHandler _moveHandler;
        private CastlingAvailability _castlingAvailability;
        public ICastlingAvailability CastlingAvailability { get => _castlingAvailability; private set { _castlingAvailability = (CastlingAvailability)value; } }
        private EnPassantAvailability _enPassantAvailability;
        public IEnPassantAvailability EnPassantAvailability { get => _enPassantAvailability; private set { _enPassantAvailability = (EnPassantAvailability)value; } }
        public Color Turn { get; private set; }
        public int HalfMove { get; private set; }
        public int FullMove { get; private set; }
        public string LastPosition { get; private set; }

        public Chessboard(string position)
        {
            FenInfo fenPosition = new(position);
            _board = CreateBoard();
            SetPiecesOnBoard(fenPosition.Board);
            Turn = fenPosition.ConvertTurn(fenPosition.Turn);
            _castlingAvailability = fenPosition.ConvertCastling(fenPosition.CastlingAvailability);
            Coordinate? enPassantPosition = fenPosition.ConvertEnPassant(fenPosition.EnPassantAvailability);
            _enPassantAvailability = new(enPassantPosition);
            _enPassantHandler = new(this);
            HalfMove = fenPosition.ConvertMoveAmount(fenPosition.HalfMove);
            FullMove = fenPosition.ConvertMoveAmount(fenPosition.FullMove);
            LastPosition = position;
            _promotionHandler = new(this);
            _castlingHandler = new(this);
            _moveHandler = SetupMoveHandlerChain();
        }
        public Piece? AddPiece(Coordinate position, char piece, Color player)
        {
            Piece createdPiece = CreatePiece(piece, position, player);
            Piece? removedPiece = RemovePiece(position);
            GetSquare(position).Piece = createdPiece;

            return removedPiece;
        }

        public Piece? RemovePiece(Coordinate position)
        {
            Square square = GetSquare(position);
            if (!square.HasPiece) return null;
            Piece piece = square.Piece!;
            square.Piece = null;

            return piece;
        }

        public IReadOnlySquare GetReadOnlySquare(string coordinate)
        {
            Coordinate origin = Coordinate.GetInstance(coordinate);
            return GetSquare(origin);
        }

        public IReadOnlySquare GetReadOnlySquare(Coordinate coordinate)
        {
            return GetSquare(coordinate);
        }

        public Square GetSquare(Coordinate coordinate)
        {
            return _board[coordinate.RowToInt][coordinate.ColumnToInt];
        }

        public MovePlayed MovePiece(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (!GetReadOnlySquare(origin).HasPiece) { throw new ChessboardException($"No piece was found in coordinate {origin}!"); }
            if (GetReadOnlySquare(origin).ReadOnlyPiece!.Color != Turn) { throw new ChessboardException("It's not your turn"); }
            MovePlayed move = _moveHandler.Handle(origin, destination, promotingPiece);

            HandleIllegalPosition();
            _enPassantAvailability.ClearEnPassant();
            _enPassantAvailability.SetVulnerablePawn(move.PieceMoved);
            _castlingAvailability.UpdateAvailability(origin, Turn);
            SwitchTurns();

            return move;
        }

        public List<Coordinate> GetPiecesPosition(List<Coordinate> range)
        {
            return range.FindAll(c => GetReadOnlySquare(c).HasPiece).ToList();
        }

        public List<Coordinate> GetPiecesPosition(Color player)
        {
            List<Coordinate> piecePosition = new();

            _board.ForEach(action: r =>
            {
                var squares = r.FindAll(c => c.ReadOnlyPiece?.Color == player);
                squares.ForEach(s => piecePosition.Add(s.Coordinate));
            });

            return piecePosition;
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
            Turn = ColorUtils.GetOppositeColor(Turn);
        }

        private void HandleIllegalPosition()
        {
            Check checkHandler = new(this);
            if (checkHandler.IsInCheck(Turn)) { RestoreToLastPosition(); throw new ChessboardException("Invalid move!"); }
        }

        private void RestoreToLastPosition()
        {
            Chessboard previous = new(LastPosition);
            _board = previous._board;
            Turn = previous.Turn;
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
            _castlingHandler.SetNext(new DefaultMoveHandler(this));
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
            return Turn == Color.Black ? "b" : "w";
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

        private List<List<Square>> CreateBoard()
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

        private Piece CreatePiece(char type, Coordinate origin)
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

        private Piece CreatePiece(char type, Coordinate origin, Color player)
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