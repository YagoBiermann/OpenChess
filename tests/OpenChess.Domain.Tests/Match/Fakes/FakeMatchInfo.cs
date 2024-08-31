using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class FakeMatch
    {
        public static MatchInfo RestoreMatch(string fen, string mId, string p1Id, string p2Id, long p1TimeRemaining, long p2TimeRemaining, string currentTurnStartedAt, int mtime = 5, string mstatus = "InProgress", string? winner = null)
        {
            string matchId = mId;
            string player1Id = p1Id;
            string player2Id = p2Id;

            PlayerInfo player1 = new(player1Id, 'w', matchId, p1TimeRemaining);
            PlayerInfo player2 = new(player2Id, 'b', matchId, p2TimeRemaining);
            List<PlayerInfo> players = [player1, player2];
            var status = mstatus;
            var time = mtime;
            List<string> pgnMoves = ["2. d5", "1. e4"];
            MatchInfo matchInfo = new(matchId, players, fen, pgnMoves, status, time, currentTurnStartedAt, DateTime.UtcNow.ToString(), winner);

            return matchInfo;
        }

        public static Match RestoreMatch(string fen)
        {
            string matchId = Guid.NewGuid().ToString();
            string player1Id = Guid.NewGuid().ToString();
            string player2Id = Guid.NewGuid().ToString();
            long fiveMinutes = TimeSpan.FromMinutes((int)new Time(5)).Ticks;

            PlayerInfo player1 = new(player1Id, 'w', matchId, fiveMinutes);
            PlayerInfo player2 = new(player2Id, 'b', matchId, fiveMinutes);
            List<PlayerInfo> players = new() { player1, player2 };
            var status = MatchStatus.InProgress.ToString();
            var time = 5;
            var pgnMoves = new List<string>();
            string currentTurnStartedAt = DateTime.UtcNow.ToString();
            MatchInfo matchInfo = new(matchId, players, fen, pgnMoves, status, time, currentTurnStartedAt, DateTime.UtcNow.ToString());
            Match match = new(matchInfo);

            return match;
        }

        public static Match RestoreAndPlay(string fen, string origin, string destination, string? promoting = null, int time = 5)
        {
            string matchId = Guid.NewGuid().ToString();
            long fiveMinutes = TimeSpan.FromMinutes(new Time(time)).Ticks;
            PlayerInfo player1 = new(Guid.NewGuid().ToString(), 'w', matchId, fiveMinutes);
            PlayerInfo player2 = new(Guid.NewGuid().ToString(), 'b', matchId, fiveMinutes);
            List<PlayerInfo> players = [player1, player2];
            string currentTurnStartedAt = DateTime.UtcNow.ToString();
            MatchInfo matchInfo = new(matchId, players, fen, new(), MatchStatus.InProgress.ToString(), time, currentTurnStartedAt, DateTime.UtcNow.ToString());
            Match match = new(matchInfo);
            Guid currentPlayer = match.CurrentPlayerInfo!.Id;
            match.Play(new(currentPlayer, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination), promoting));

            return match;
        }
    }
}