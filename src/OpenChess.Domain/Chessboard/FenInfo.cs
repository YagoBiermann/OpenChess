using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("OpenChess.Test")]
namespace OpenChess.Domain
{
    internal record FenInfo
    {
        public string Board;
        public string Turn;
        public string CastlingAvailability;
        public string EnPassantAvailability;
        public string HalfMove;
        public string FullMove;
        public static string InitialPosition { get => "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; }

        public FenInfo(string position)
        {
            if (!IsValid(position)) throw new ChessboardException("Invalid FEN string!");
            List<string> fields = position.Split(" ").ToList();
            Board = fields[0];
            Turn = fields[1];
            CastlingAvailability = fields[2];
            EnPassantAvailability = fields[3];
            HalfMove = fields[4];
            FullMove = fields[5];
        }

        public static bool IsValid(string position)
        {
            bool hasSixFields = HasSixFields(position);
            bool hasEightColumns = HasEightColumns(position);
            bool hasDuplicatedSlashes = HasDuplicatedSlashes(position);
            if (!hasSixFields || !hasEightColumns || hasDuplicatedSlashes) return false;

            List<string> fields = position.Split(" ").ToList();
            List<string> rows = fields[0].Split("/").ToList();
            bool hasValidRows = !rows.SkipWhile(IsValidRow).Any();
            if (!hasValidRows) return false;

            bool isValidActiveColor = IsValidActiveColor(fields[1]);
            bool isValidCastling = IsValidCastlingField(fields[2]);
            bool isValidEnPassant = IsValidEnPassant(fields[3]);
            bool isValidHalfMove = IsValidHalfMove(fields[4]);
            bool isValidFullMove = IsValidFullMove(fields[5]);

            return isValidActiveColor && isValidCastling && isValidEnPassant && isValidHalfMove && isValidFullMove;
        }

        public static string BuildFenString(IReadOnlyChessboard chessboard)
        {
            string chessboardString = BuildChessboardString(chessboard);
            string turn = BuildTurnString(chessboard);
            string castling = BuildCastlingString(chessboard);
            string enPassant = BuildEnPassantString(chessboard);

            return $"{chessboardString} {turn} {castling} {enPassant} {chessboard.HalfMove} {chessboard.FullMove}";
        }

        public Color ConvertTurn(string field)
        {
            return char.Parse(field) == 'w' ? Color.White : Color.Black;
        }

        public CastlingAvailability ConvertCastling(string field)
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

        public static EnPassantAvailability ConvertEnPassant(string field)
        {
            return new EnPassantAvailability(field == "-" ? null : Coordinate.GetInstance(field));
        }

        public static int ConvertMoveAmount(string field)
        {
            return int.Parse(field);
        }

        private static string BuildEnPassantString(IReadOnlyChessboard chessboard)
        {
            return chessboard.EnPassantAvailability.EnPassantPosition?.ToString() ?? "-";
        }

        private static string BuildCastlingString(IReadOnlyChessboard chessboard)
        {
            bool isWhiteKingSideAvailable = chessboard.CastlingAvailability.IsWhiteKingSideAvailable;
            bool IsWhiteQueenSideAvailable = chessboard.CastlingAvailability.IsWhiteQueenSideAvailable;
            bool isBlackKingSideAvailable = chessboard.CastlingAvailability.IsBlackKingSideAvailable;
            bool IsBlackQueenSideAvailable = chessboard.CastlingAvailability.IsBlackQueenSideAvailable;
            string castlingAvailability = "";
            if (isWhiteKingSideAvailable) castlingAvailability += "K";
            if (IsWhiteQueenSideAvailable) castlingAvailability += "Q";
            if (isBlackKingSideAvailable) castlingAvailability += "k";
            if (IsBlackQueenSideAvailable) castlingAvailability += "q";
            if (!isWhiteKingSideAvailable && !IsWhiteQueenSideAvailable && !isBlackKingSideAvailable && !IsBlackQueenSideAvailable) castlingAvailability += "-";

            return castlingAvailability;
        }

        private static string BuildTurnString(IReadOnlyChessboard chessboard)
        {
            return chessboard.CurrentPlayer == Color.Black ? "b" : "w";
        }

        private static string BuildChessboardString(IReadOnlyChessboard chessboard)
        {
            string chessboardString = "";

            for (int row = 7; row >= 0; row--)
            {
                string currentRow = "";
                currentRow += BuildRowString(row, chessboard);
                chessboardString += currentRow;

                if (row == 0) { continue; }
                chessboardString += "/";
            }

            return chessboardString;
        }

        private static string BuildRowString(int row, IReadOnlyChessboard chessboard)
        {
            string builtRow = "";
            int amount = 0;
            for (int col = 0; col <= 7; col++)
            {
                IReadOnlySquare currentSquare = chessboard.GetReadOnlySquare(Coordinate.GetInstance(col, row).ToString());
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

        private static bool HasSixFields(string value)
        {
            int fields = value.Split(" ").Count();
            bool hasValidFields = fields == 6;
            return hasValidFields;
        }

        private static bool HasEightColumns(string value)
        {
            int columns = value.Split("/").Length;
            bool result = columns == 8;
            return result;
        }

        private static bool HasDuplicatedSlashes(string value)
        {
            bool hasDuplicatedSlashes = Regex.IsMatch(value, "//+");
            return hasDuplicatedSlashes;
        }

        private static bool IsValidRow(string value)
        {
            string pieces = Regex.Replace(value, @"\d", "");
            int emptySquareSum = value.Where(char.IsDigit).Select(v => int.Parse(v.ToString())).Sum();

            if (!HasValidPieces(pieces) && emptySquareSum < 8) { return false; }

            int piecesCount = pieces.Count();
            int totalSum = emptySquareSum + piecesCount;
            bool isValidRow = totalSum == 8;

            return isValidRow;
        }

        private static bool HasValidPieces(string value)
        {
            Regex rx = new Regex(@"^[rnbqkbnrp]+$", RegexOptions.IgnoreCase);
            return rx.IsMatch(value);
        }

        private static bool IsValidActiveColor(string value)
        {
            Regex rx = new(@"^(w|b)$", RegexOptions.None);
            return rx.IsMatch(value);
        }

        private static bool IsValidEnPassant(string value)
        {
            Regex rx = new(@"^([A-H](3|6))|(-)$", RegexOptions.None);
            return rx.IsMatch(value);
        }

        private static bool IsValidCastlingField(string value)
        {
            Regex rx = new Regex(@"^-$|^K?Q?k?q?$", RegexOptions.None);
            return rx.IsMatch(value);
        }
        private static bool IsValidHalfMove(string value)
        {
            Regex rx = new(@"^(?:[0-9]\d?|100)$", RegexOptions.None);
            return rx.IsMatch(value);
        }

        private static bool IsValidFullMove(string value)
        {
            Regex rx = new(@"^(?:[1-9]\d{0,2}|1000)$", RegexOptions.None);
            return rx.IsMatch(value);
        }
    }
}