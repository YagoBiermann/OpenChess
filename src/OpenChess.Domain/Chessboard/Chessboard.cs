namespace OpenChess.Domain
{
    internal class Chessboard : IReadOnlyChessboard
    {
        private readonly List<List<Square>> _board;
        public Color Turn { get; private set; }
        public CastlingAvailability CastlingAvailability { get; set; }
        public EnPassant EnPassant { get; private set; }
        public int HalfMove { get; set; }
        public int FullMove { get; set; }
        public string LastPosition { get; }

        public Chessboard(string position)
        {
            FEN fenPosition = new(position);
            _board = CreateBoard();
            SetPiecesOnBoard(fenPosition.Board);
            Turn = ConvertTurn(fenPosition.Turn);
            CastlingAvailability = ConvertCastling(fenPosition.CastlingAvailability);
            Coordinate? enPassantPosition = ConvertEnPassant(fenPosition.EnPassantAvailability);
            EnPassant = new(enPassantPosition, this);
            HalfMove = ConvertMoveAmount(fenPosition.HalfMove);
            FullMove = ConvertMoveAmount(fenPosition.FullMove);
            LastPosition = position;
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

        public Piece? ChangePiecePosition(Coordinate origin, Coordinate destination)
        {
            if (!GetReadOnlySquare(origin).HasPiece) { throw new ChessboardException($"No piece was found in coordinate {origin}!"); }

            Square originSquare = GetSquare(origin);
            Square destinationSquare = GetSquare(destination);
            Piece piece = originSquare.Piece!;
            Piece? capturedPiece = destinationSquare.Piece;
            originSquare.Piece = null;
            destinationSquare.Piece = piece;

            return capturedPiece;
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

        private IReadOnlyPiece? HandleEnPassant(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece? piece = GetReadOnlySquare(origin).ReadOnlyPiece;
            if (piece is not Pawn pawn) throw new ChessboardException("Cannot handle en passant because piece is not a pawn.");
            if (!pawn.CanCaptureByEnPassant) throw new ChessboardException("This pawn cannot capture by en passant!");

            HandleDefault(origin, destination);
            Coordinate vulnerablePawnPosition = EnPassant.GetVulnerablePawn!.Origin;
            RemovePiece(vulnerablePawnPosition);

            return piece;
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
        private Color ConvertTurn(string field)
        {
            return char.Parse(field) == 'w' ? Color.White : Color.Black;
        }

        private CastlingAvailability ConvertCastling(string field)
        {
            Dictionary<char, bool> pairs = new()
            {
                {'K', false},
                {'Q', false},
                {'k', false},
                {'q', false},
            };

            foreach (char letter in field)
            {
                if (letter == 'K') pairs[letter] = true;
                if (letter == 'Q') pairs[letter] = true;
                if (letter == 'k') pairs[letter] = true;
                if (letter == 'q') pairs[letter] = true;
            }

            return new CastlingAvailability(pairs['K'], pairs['Q'], pairs['k'], pairs['q']);
        }

        private Coordinate? ConvertEnPassant(string field)
        {
            if (field == "-") return null;
            return Coordinate.GetInstance(field);
        }

        private int ConvertMoveAmount(string field)
        {
            return int.Parse(field);
        }
    }
}