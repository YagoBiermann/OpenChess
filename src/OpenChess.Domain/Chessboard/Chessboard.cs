namespace OpenChess.Domain
{
    internal class Chessboard
    {
        private readonly List<List<Square>> _board;
        public Color Turn { get; set; }
        public CastlingAvailability CastlingAvailability { get; set; }
        public Coordinate? EnPassant { get; set; }
        public int HalfMove { get; set; }
        public int FullMove { get; set; }

        public Chessboard(string position)
        {
            FEN fenPosition = new(position);
            _board = CreateBoard();
            SetPiecesOnBoard(fenPosition.Board);
            Turn = ConvertTurn(fenPosition.Turn);
            CastlingAvailability = ConvertCastling(fenPosition.CastlingAvailability);
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
                int currentRow = rows.IndexOf(row);
                int currentCol = 0;
                foreach (char col in row)
                {

                    if (!char.IsDigit(col))
                    {
                        Coordinate origin = Coordinate.GetInstance(currentCol, currentRow);
                        Piece piece = Piece.Create(col, origin);
                        GetSquare(origin).Piece = piece;

                        currentCol++;
                        continue;
                    }

                    currentCol += int.Parse(col.ToString());
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