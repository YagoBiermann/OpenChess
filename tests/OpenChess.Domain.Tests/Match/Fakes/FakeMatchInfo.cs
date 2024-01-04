using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class FakeMatch
    {
        public static MatchInfo RestoreMatch(string fen, string mId, string p1Id, string p2Id, int mtime = 5, string mstatus = "InProgress", string? winner = null)
        {
            string matchId = mId;
            string player1Id = p1Id;
            string player2Id = p2Id;

            PlayerInfo player1 = new(player1Id, 'w', matchId);
            PlayerInfo player2 = new(player2Id, 'b', matchId);
            List<PlayerInfo> players = new() { player1, player2 };
            var status = mstatus;
            var time = mtime;
            List<string> pgnMoves = new() { "2. d5", "1. e4" };
            var pgnStack = new Stack<string>(pgnMoves);
            MatchInfo matchInfo = new(matchId, players, fen, pgnStack, status, time, winner);

            return matchInfo;
        }

        public static MatchInfo RestoreMatch(string fen)
        {
            string matchId = Guid.NewGuid().ToString();
            string player1Id = Guid.NewGuid().ToString();
            string player2Id = Guid.NewGuid().ToString();

            PlayerInfo player1 = new(player1Id, 'w', matchId);
            PlayerInfo player2 = new(player2Id, 'b', matchId);
            List<PlayerInfo> players = new() { player1, player2 };
            var status = MatchStatus.InProgress.ToString();
            var time = 5;
            var pgnStack = new Stack<string>();
            MatchInfo matchInfo = new(matchId, players, fen, pgnStack, status, time);

            return matchInfo;
        }

        public static Match RestoreAndPlay(string fen, string origin, string destination, string? promoting = null)
        {
            string matchId = Guid.NewGuid().ToString();
            PlayerInfo player1 = new(Guid.NewGuid().ToString(), 'w', matchId);
            PlayerInfo player2 = new(Guid.NewGuid().ToString(), 'b', matchId);
            List<PlayerInfo> players = new() { player1, player2 };
            MatchInfo matchInfo = new(matchId, players, fen, new(), MatchStatus.InProgress.ToString(), 5);
            Match match = new(matchInfo);
            Guid currentPlayer = match.CurrentPlayerInfo!.Value.Id;
            match.Play(new(currentPlayer, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination), promoting));

            return match;
        }
    }
}