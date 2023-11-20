namespace OpenChess.Domain
{
    internal class Chessboard
    {
        private readonly List<List<Square>> _board;
        public Color Turn { get; set; }
        public HashSet<CastlingRights> Castling { get; } = new();
        public Coordinate? EnPassant { get; set; }
        public int HalfMove { get; set; }
        public int FullMove { get; set; }

        public Chessboard(string position)
        {
            FEN fenPosition = new(position);
            _board = CreateBoard();
            SetPiecesOnBoard(fenPosition.Board);
            Turn = ConvertTurn(fenPosition.Turn);
            ConvertCastling(fenPosition.CastlingAvailability);
            EnPassant = ConvertEnPassant(fenPosition.EnPassantAvailability);
            HalfMove = ConvertMoveAmount(fenPosition.HalfMove);
            FullMove = ConvertMoveAmount(fenPosition.FullMove);
        }

        public Square GetSquare(Coordinate coordinate)
        {
            return _board[coordinate.ColumnToInt][coordinate.RowToInt];
        }

        public void ChangePiecePosition(Coordinate origin, Coordinate destination)
        {
            if (!GetSquare(origin).HasPiece) { throw new ChessboardException($"No piece was found in coordinate {origin}!"); }

            Square originSquare = GetSquare(origin);
            Square destinationSquare = GetSquare(destination);
            Piece piece = originSquare.Piece!;
            originSquare.RemovePiece();
            destinationSquare.Piece = piece;
        }

        private List<List<Square>> CreateBoard()
        {
            List<List<Square>> board = new();
            for (int col = 0; col <= 7; col++)
            {
                List<Square> rows = new();
                for (int row = 0; row <= 7; row++)
                {
                    rows.Add(new(Coordinate.GetInstance(col, row), null));
                }
                board.Add(rows);
            }

            return board;
        }

        private void SetPiecesOnBoard(string field)
        {
            List<string> rows = field.Split("/").Reverse().ToList();
            foreach (string row in rows)
            {
                int currentCol = rows.IndexOf(row);
                int emptySquareSum = 0;
                foreach (char col in row)
                {

                    if (!char.IsDigit(col))
                    {
                        Coordinate origin = Coordinate.GetInstance(emptySquareSum, currentCol);
                        Piece piece = Piece.Create(col, origin);
                        GetSquare(origin).Piece = piece;

                        emptySquareSum++;
                        continue;
                    }

                    emptySquareSum += int.Parse(col.ToString());
                }
            }

        }
        private Color ConvertTurn(string field)
        {
            return char.Parse(field) == 'w' ? Color.White : Color.Black;
        }

        private void ConvertCastling(string field)
        {

            Dictionary<char, CastlingRights> pairs = new()
            {
                {'Q', CastlingRights.WhiteQueenSide},
                {'K', CastlingRights.WhiteKingSide},
                {'q', CastlingRights.BlackQueenSide},
                {'k', CastlingRights.BlackKingSide},
                {'-', CastlingRights.None}
            };

            if (field == "-") Castling.Add(CastlingRights.None);
            foreach (char letter in field)
            {
                if (letter == '-') continue;
                Castling.Add(pairs[letter]);
            }
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