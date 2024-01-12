namespace OpenChess.Domain
{
    internal class Clock
    {
        public TimeSpan TimeRemaining { get; private set; }
        public DateTime CurrentTurnStartedAt { get; private set; }
        public DateTime CurrentMovePlayedAt { get; private set; }

        public Clock(DateTime currentTurnStartedAt, long timeRemaining)
        {
            TimeRemaining = TimeSpan.FromTicks(timeRemaining);
            CurrentTurnStartedAt = currentTurnStartedAt;
            CurrentMovePlayedAt = DateTime.UtcNow;
        }

        public TimeSpan CalculateTimeRemainingForCurrentPlayer()
        {
            var turnDuration = CurrentMovePlayedAt - CurrentTurnStartedAt;
            var newTimeRemaining = TimeRemaining - turnDuration;

            return newTimeRemaining;
        }
    }
}