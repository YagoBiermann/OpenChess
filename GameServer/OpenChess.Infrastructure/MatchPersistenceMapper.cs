using OpenChess.Domain;

namespace OpenChess.Infrastructure
{
    internal static class MatchPersistenceMapper
    {
        public static IMatch ToMatch(MatchPersistenceModel matchPM)
        {
            List<PlayerInfo> players = [];
            foreach (var playerPM in matchPM.Players)
            {
                PlayerInfo player = new(playerPM.Id, playerPM.Color, playerPM.CurrentMatch, playerPM.TimeRemaining);
                players.Add(player);
            }

            MatchInfo matchInfo = new(matchPM.MatchId, players, matchPM.Fen, matchPM.PgnMoves, matchPM.Status, matchPM.Time, matchPM.CurrentTurnStartedAt, matchPM.CreatedAt, matchPM.Winner);
            IMatch match = new Match(matchInfo);

            return match;
        }
        public static MatchPersistenceModel ToPersistenceModel(IMatch match)
        {
            List<PlayerPersistenceModel> playerPersistenceModels = [];
            foreach (var player in match.Players)
            {
                PlayerPersistenceModel playerPersistenceModel = new(player.Id.ToString(), player.Color.Value, player.CurrentMatch.ToString(), player.TimeRemaining.Ticks);
                playerPersistenceModels.Add(playerPersistenceModel);
            }

            MatchPersistenceModel matchPersistenceModel = new(
             match.Id.ToString(),
             playerPersistenceModels,
             match.Fen,
             (List<string>)match.PgnMoves,
             match.Status.ToString(),
             match.Duration,
             match.Winner?.Value,
             match.CurrentTurnStartedAt.ToString(),
             match.CreatedAt.ToString()
            );

            return matchPersistenceModel;
        }
    }
}