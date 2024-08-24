using Newtonsoft.Json;

namespace MatchMaking
{
    public class UserDTO
    {
        [JsonProperty("MatchTime")]
        public int MatchTime { get; set; }

        [JsonProperty("Score")]
        public int Score { get; set; }
    }
}