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
        public ICastlingAvailability CastlingAvailability { get; private set; }
        public IEnPassantAvailability EnPassantAvailability { get; private set; }
        public IMoveCalculator MovesCalculator { get; }
        public int HalfMove { get; private set; }
        public int FullMove { get; private set; }
        public string LastPosition { get; private set; }

        public Chessboard(FenInfo fenInfo)
        {
            _board = CreateBoard();
            SetPiecesOnBoard(fenInfo.Board);
            CastlingAvailability = FenInfo.ConvertCastling(fenInfo.CastlingAvailability);
            EnPassantAvailability = FenInfo.ConvertEnPassant(fenInfo.EnPassantAvailability);
            LastPosition = fenInfo.Position;
            MovesCalculator = new MovesCalculator(this);
            _enPassantHandler = new(this, MovesCalculator);
            _promotionHandler = new(this, MovesCalculator);
            _castlingHandler = new(this, MovesCalculator);
            _moveHandler = SetupMoveHandlerChain();
            HalfMove = FenInfo.ConvertMoveAmount(fenInfo.HalfMove);
            FullMove = FenInfo.ConvertMoveAmount(fenInfo.FullMove);
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

        public IReadOnlyPiece? ChangePiecePosition(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece? piece = RemovePiece(origin) ?? throw new ChessboardException($"Piece not found at origin: {origin}");
            IReadOnlyPiece? capturedPiece = RemovePiece(destination);
            AddPiece(destination, piece.Name, piece.Color);

            return capturedPiece;
        }

            _piecesCache.Clear();

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

        private IMoveHandler SetupMoveHandlerChain()
        {
            _promotionHandler.SetNext(_enPassantHandler);
            _enPassantHandler.SetNext(_castlingHandler);
            _castlingHandler.SetNext(new DefaultMoveHandler(this, MovesCalculator));
            return _promotionHandler;
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