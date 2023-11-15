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

            return hasSixFields && hasEightColumns;
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

    }
}