using BlogApplication.Filters;
using BlogApplication.Filters.Authorization;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [ApiController]
    [AuthenticationFilter]
    [ExceptionFilter]
    [AdminAuthorizationFilter]
    [Route("api/ranking")]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService services;

        public RankingController(IRankingService services)
        {
            this.services = services;
        }

        [HttpGet("activity")]
        public IActionResult GetUserActivityRanking([FromQuery] DateRange range) 
        {
            UserScore[] ranking = services.GetUserActivityRanking(range);
            OutModelUserScore[] rankingOut = ranking.Select(u => new OutModelUserScore(u)).ToArray();
            return Ok(rankingOut);
        }

        [HttpGet("offenses")]
        public IActionResult GetUserOffensesRanking([FromQuery] DateRange range)
        {
            UserScore[] ranking = services.GetUserOffensesRanking(range);
            OutModelUserScore[] rankingOut = ranking.Select(u => new OutModelUserScore(u)).ToArray();
            return Ok(rankingOut);
        }
    }
}
