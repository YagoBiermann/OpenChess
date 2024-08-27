namespace OpenChess.Domain
{
    public readonly struct Time
    {
        private static readonly List<int> AllowedTimes = [3, 5, 10, 15, 30];
        private readonly int _time;

        public Time(int time)
        {
            if (IsValid(time))
            {
                _time = time;
                return;
            }
            throw new MatchException($"Invalid time value: {time}. Allowed values are: {string.Join(", ", AllowedTimes)}.");
        }

        public int Value
        {
            get => _time;
        }

        public static bool IsValid(int value)
        {
            return AllowedTimes.Contains(value);
        }

        public static implicit operator int(Time time) => time._time;

        public override readonly string ToString() => _time.ToString();
    }
}