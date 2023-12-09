namespace OpenChess.Domain
{
    internal class Chessboard : IReadOnlyChessboard
    {
        private List<List<Square>> _board;
        private Promotion _promotion;
        public Color Turn { get; private set; }
        public CastlingAvailability CastlingAvailability { get; set; }
        public EnPassant EnPassant { get; private set; }
        public int HalfMove { get; set; }
        public int FullMove { get; set; }
        public string LastPosition { get; private set; }

        public Chessboard(string position)
        {
            FenInfo fenPosition = new(position);
            _board = CreateBoard();
            SetPiecesOnBoard(fenPosition.Board);
            Turn = fenPosition.ConvertTurn(fenPosition.Turn);
            CastlingAvailability = fenPosition.ConvertCastling(fenPosition.CastlingAvailability);
            Coordinate? enPassantPosition = fenPosition.ConvertEnPassant(fenPosition.EnPassantAvailability);
            EnPassant = new(enPassantPosition, this);
            HalfMove = fenPosition.ConvertMoveAmount(fenPosition.HalfMove);
            FullMove = fenPosition.ConvertMoveAmount(fenPosition.FullMove);
            LastPosition = position;
            _promotion = new(this);
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

        private Square GetSquare(Coordinate coordinate)
        {
            return _board[coordinate.RowToInt][coordinate.ColumnToInt];
        }

        public Piece? RemovePiece(Coordinate position)
        {
            Square square = GetSquare(position);
            if (!square.HasPiece) return null;
            Piece piece = square.Piece!;
            square.Piece = null;

            return piece;
        }

        public IReadOnlyPiece? MovePiece(Coordinate origin, Coordinate destination)
        {
            if (!GetReadOnlySquare(origin).HasPiece) { throw new ChessboardException($"No piece was found in coordinate {origin}!"); }
            if (!IsLegalMove(origin, destination)) throw new ChessboardException("Invalid move!");
            IReadOnlyPiece? capturedPiece;
            if (EnPassant.IsEnPassantMove(origin, destination))
            {
                capturedPiece = HandleEnPassant(origin, destination);
                UpdateState(destination);
                return capturedPiece;
            }
            else
            {
                capturedPiece = HandleDefault(origin, destination);
                UpdateState(destination);
                return capturedPiece;
            }
        }

        public List<Coordinate> GetPiecesPosition(List<Coordinate> range)
        {
            return range.FindAll(c => GetReadOnlySquare(c).HasPiece).ToList();
        }
        public void SwitchTurns()
        {
            Turn = ColorUtils.GetOppositeColor(Turn);
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

        private IReadOnlyPiece? HandleDefault(Coordinate origin, Coordinate destination)
        {
            Square originSquare = GetSquare(origin);
            Square destinationSquare = GetSquare(destination);
            Piece piece = originSquare.Piece!;
            Piece? capturedPiece = destinationSquare.Piece;
            originSquare.Piece = null;
            destinationSquare.Piece = piece;

            return capturedPiece;
        }

        private IReadOnlyPiece? HandleEnPassant(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece? piece = GetReadOnlySquare(origin).ReadOnlyPiece;
            if (piece is not Pawn pawn) throw new ChessboardException("Cannot handle en passant because piece is not a pawn.");
            if (!pawn.CanCaptureByEnPassant) throw new ChessboardException("This pawn cannot capture by en passant!");

            HandleDefault(origin, destination);
            Coordinate vulnerablePawnPosition = EnPassant.GetVulnerablePawn!.Origin;
            IReadOnlyPiece? capturedPiece = RemovePiece(vulnerablePawnPosition);

            return capturedPiece;
        }

        private bool IsLegalMove(Coordinate origin, Coordinate destination)
        {
            List<MoveDirections> legalMoves = GetReadOnlySquare(origin).ReadOnlyPiece!.CalculateLegalMoves();
            return legalMoves.Exists(m => m.Coordinates.Contains(destination));
        }
        private void HandleIllegalPosition()
        {
            if (Check.IsInCheck(Turn, this)) { RestoreToLastPosition(); throw new ChessboardException("Invalid move!"); }
        }
        private void RestoreToLastPosition()
        {
            Chessboard previous = new(LastPosition);
            _board = previous._board;
            Turn = previous.Turn;
            EnPassant = previous.EnPassant;
            CastlingAvailability = previous.CastlingAvailability;
            HalfMove = previous.HalfMove;
            FullMove = previous.FullMove;
            LastPosition = previous.LastPosition;
        }
        private void UpdateState(Coordinate movedPiecePosition)
        {
            EnPassant.HandleUpdate(movedPiecePosition);
            HandleIllegalPosition();
            SwitchTurns();
        }
        private string BuildEnPassantString()
        {
            return EnPassant.Position is null ? "-" : EnPassant.ToString();
        }
        private string BuildCastlingString()
        {
            return CastlingAvailability.ToString();
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
                'K' => new King(color, origin, this),
                'Q' => new Queen(color, origin, this),
                'R' => new Rook(color, origin, this),
                'B' => new Bishop(color, origin, this),
                'N' => new Knight(color, origin, this),
                'P' => new Pawn(color, origin, this),
                _ => throw new PieceException($"{type} does not represent a piece"),
            };
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