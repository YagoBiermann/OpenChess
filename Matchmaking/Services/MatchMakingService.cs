using System.Text;
using MatchMaking.Models;
using Newtonsoft.Json;
using Redis.OM;
using Redis.OM.Searching;

public class MatchMakingService : BackgroundService
{
    private readonly RedisConnectionProvider _provider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public MatchMakingService(RedisConnectionProvider provider, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _configuration = configuration;
        _provider = provider;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var ticketCollection = _provider.RedisCollection<Ticket>();

        while (!stoppingToken.IsCancellationRequested)
        {
            // Check and update timed out tickets
            await HandleTimedOutTicketsAsync(ticketCollection);

            // Remove tickets that timed out more than 10 minutes ago
            await RemoveOldTimedOutTicketsAsync(ticketCollection);

            // Only start matching if there are at least 2 tickets
            var ticketsCount = ticketCollection.Count(x => x.Status == Status.Searching);
            if (ticketsCount >= 2)
            {
                await MatchTicketsAsync(ticketCollection);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task HandleTimedOutTicketsAsync(IRedisCollection<Ticket> ticketCollection)
    {
        var timedOutTickets = ticketCollection.Where(t => t.Status == Status.Searching).AsEnumerable().Where(t => t.HasTimedOut()).ToList();

        foreach (var ticket in timedOutTickets)
        {
            ticket.Status = Status.TimedOut;
            await ticketCollection.UpdateAsync(ticket);
        }
    }

    private async Task RemoveOldTimedOutTicketsAsync(IRedisCollection<Ticket> ticketCollection)
    {
        var oldTimedOutTickets = ticketCollection.Where(t => t.Status == Status.TimedOut).AsEnumerable().Where(t => t.TimedOutMoreThan10MinutesAgo()).ToList();

        foreach (var ticket in oldTimedOutTickets)
        {
            await ticketCollection.DeleteAsync(ticket);
        }
    }

    private async Task MatchTicketsAsync(IRedisCollection<Ticket> ticketCollection)
    {
        var tickets = ticketCollection.Where(t => t.Status == Status.Searching)
                                      .OrderBy(t => t.Score)
                                      .ToList();

        var groupedTickets = tickets.GroupBy(t => t.MatchTime);

        foreach (var group in groupedTickets)
        {
            var sortedTickets = group.ToList();

            for (int i = 0; i < sortedTickets.Count - 1; i += 2)
            {
                var ticket1 = sortedTickets[i];
                var ticket2 = sortedTickets[i + 1];

                if (ticket1.Score == ticket2.Score || Math.Abs(ticket1.Score - ticket2.Score) <= 100)
                {
                    ticket1.Status = Status.Found;
                    ticket2.Status = Status.Found;

                    var matchId = await CreateMatchAsync(ticket1.MatchTime);
                    ticket1.MatchId = matchId;
                    ticket2.MatchId = matchId;

                    await ticketCollection.UpdateAsync(ticket1);
                    await ticketCollection.UpdateAsync(ticket2);
                }
            }
        }
    }

    private async Task<Guid?> CreateMatchAsync(int time)
    {
        var client = _httpClientFactory.CreateClient();

        string endpoint = _configuration["ApiSettings:Endpoints:CreateMatch"] ?? throw new InvalidOperationException("CreateMatch endpoint is not configured.");
        var body = new { time };
        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{endpoint}", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MatchResponse>(responseContent);
            return result?.MatchId;
        }

        return null;
    }

    private class MatchResponse
    {
        public Guid? MatchId { get; set; }
    }
}