using OpenChess.Domain;

namespace OpenChess.Infrastructure
{
    internal static class MatchPersistenceMapper
    {
        public static Match ToMatch(MatchPersistenceModel matchPM)
        {
            List<PlayerInfo> players = [];
            foreach (var playerPM in matchPM.Players)
            {
                PlayerInfo player = new(playerPM.Id, playerPM.Color, playerPM.CurrentMatch, playerPM.TimeRemaining);
                players.Add(player);
            }

            MatchInfo matchInfo = new(matchPM.MatchId, players, matchPM.Fen, matchPM.PgnMoves, matchPM.Status, matchPM.Time, matchPM.CurrentTurnStartedAt, matchPM.CreatedAt, matchPM.WinnerId);
            Match match = new(matchInfo);

            return match;
        }
        public static MatchPersistenceModel ToPersistenceModel(Match match)
        {
            List<PlayerPersistenceModel> playerPersistenceModels = [];
            foreach (var player in match.Players)
            {
                PlayerPersistenceModel playerPersistenceModel = new(player.Id.ToString(), (char)player.Color, player.CurrentMatch.ToString(), player.TimeRemaining.Ticks);
                playerPersistenceModels.Add(playerPersistenceModel);
            }

            MatchPersistenceModel matchPersistenceModel = new(
             match.Id.ToString(),
             playerPersistenceModels,
             match.Fen,
             (List<string>)match.PgnMoves,
             match.Status.ToString(),
             match.Duration,
             match.Winner.ToString(),
             match.CurrentTurnStartedAt.ToString(),
             match.CreatedAt.ToString()
            );

            return matchPersistenceModel;
        }
    }
}