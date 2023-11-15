using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OpenChess.Test")]
namespace OpenChess.Domain
{
    internal static class FEN
    {
        public static bool IsValid(string position)
        {
            bool hasSixFields = HasSixFields(position);
            return hasSixFields;
        }

        private static bool HasSixFields(string value)
        {
            int fields = value.Split(" ").Count();
            bool hasValidFields = fields == 6;
            return hasValidFields;
        }

    }
}