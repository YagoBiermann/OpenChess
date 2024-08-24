using System.Net.Mime;
using MatchMaking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Redis.OM;
using Redis.OM.Searching;

namespace MatchMaking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly RedisCollection<Ticket> _tickets;

        public TicketController(ILogger<TicketController> logger, RedisConnectionProvider provider)
        {
            _logger = logger;
            _tickets = (RedisCollection<Ticket>)provider.RedisCollection<Ticket>();
        }
    }
};