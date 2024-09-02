using OpenChess.Domain;

namespace OpenChess.Application
{
    internal static class MatchDTOMapper
    {
        public static MatchDTO ToDTO(IMatch match)
        {
            List<PlayerDTO> playersDTO = [];
            foreach (var player in match.Players)
            {
                PlayerDTO playerDTO = new(player.Id.ToString(), (char)player.Color, player.TimeRemaining.ToString());
                playersDTO.Add(playerDTO);
            }
            MatchDTO matchDTO = new(match.Id.ToString(), playersDTO, match.Fen, match.CurrentPositionStatus.ToString(), match.Winner.ToString());

            return matchDTO;
        }
    }
}