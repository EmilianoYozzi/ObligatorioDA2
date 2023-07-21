using BlogApplication.Filters;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApplication.Controllers
{
    [ApiController]
    [ExceptionFilter]
    [AuthenticationFilter]
    [Route("api/search")]
    public class SearchArticleController : ControllerBase
    {
        private readonly IArticleService services;
        public SearchArticleController(IArticleService services)
        {
            this.services = services;
        }

        [HttpGet("{keyword}")]
        public IActionResult GetArticlesByKeyword(string keyword)
        {
            Article[] articles = services.GetArticlesByKeyword(keyword);
            OutModelArticle[] articlesOut = articles.Select(a => new OutModelArticle(a)).ToArray();
            return Ok(articlesOut);
        }
    }
}
