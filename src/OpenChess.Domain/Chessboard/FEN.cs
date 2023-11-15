using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("OpenChess.Test")]
namespace OpenChess.Domain
{
    internal static class FEN
    {
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

            return isValidActiveColor && isValidCastling && isValidEnPassant && isValidHalfMove;
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
            Regex rx = new Regex(@"^[KQkq]{1,4}$|^(-{1})$", RegexOptions.None);
            return rx.IsMatch(value);
        }
        private static bool IsValidHalfMove(string value)
        {
            Regex rx = new(@"^(?:[0-9]\d?|100)$", RegexOptions.None);
            return rx.IsMatch(value);
        }
    }
}