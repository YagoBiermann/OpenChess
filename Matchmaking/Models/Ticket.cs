using Redis.OM.Modeling;

namespace MatchMaking.Models
{
    [Document(StorageType = StorageType.Json, Prefixes = ["Ticket"])]
    public class Ticket
    {
        [Indexed]
        [RedisIdField]
        public Guid Id { get; set; }

        [Indexed]
        public Guid? MatchId { get; set; }

        [Indexed(Sortable = true)]
        public int Score { get; set; }

        [Indexed]
        public int MatchTime { get; set; }

        [Indexed]
        public Status Status { get; set; } = Status.Searching;

        [Indexed]
        public TimeSpan CreatedAt { get; set; }
        public TimeSpan Interval { get; } = TimeSpan.FromSeconds(5);
        public TimeSpan Timeout { get; } = TimeSpan.FromSeconds(120);

        public Ticket() { }

        public Ticket(Guid id, int score, int time, TimeSpan createdAt, Guid? matchId = null)
        {
            MatchId = matchId;
            Id = id;
            Score = score;
            MatchTime = time;
            CreatedAt = createdAt;
        }

        public static List<int> Scores()
        {
            return
            [
                600, // beginner
                1200, // intermediate
                2000 // advanced
            ];
        }

        public static List<int> MatchAllowedTime()
        {
            // Match allowed times in Minutes
            return
            [
                5,
                10,
                15,
                30
            ];
        }

        public bool HasTimedOut()
        {
            return (CreatedAt - DateTime.UtcNow.TimeOfDay) > Timeout;
        }

        public bool TimedOutMoreThan10MinutesAgo()
        {
            return (CreatedAt - DateTime.UtcNow.TimeOfDay) > Timeout + TimeSpan.FromMinutes(10);
        }
    }
};