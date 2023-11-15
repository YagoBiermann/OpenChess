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

            return hasSixFields && hasEightColumns && !hasDuplicatedSlashes;
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
    }
}