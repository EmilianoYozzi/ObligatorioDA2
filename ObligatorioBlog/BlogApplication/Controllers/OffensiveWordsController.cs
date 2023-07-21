using BlogApplication.Filters;
using BlogApplication.Filters.Authorization;
using BlogServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [ExceptionFilter]
    [AuthenticationFilter]
    [AdminAuthorizationFilter]
    [Route("api/offensiveWords")]
    public class OffensiveWordsController : ControllerBase
    {
        private IOffensiveWordsService services;

        public OffensiveWordsController(IOffensiveWordsService service)
        {
            this.services = service;
        }

        [HttpPost]
        public IActionResult AddOffensiveWord([FromBody] string[] offensiveWords)
        {
            string[] result = services.AddOffensiveWords(offensiveWords);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteOffensiveWord([FromBody] string[] offensiveWords)
        {
            
            services.DeleteOffensiveWords(offensiveWords);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetOffensiveWords()
        {
            string[] offensiveWords = services.GetOffensiveWords();
            return Ok(offensiveWords);
        }
    }
}
