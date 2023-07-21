using BlogDomain;
using BlogApplication.Filters;
using BlogApplication.Filters.Authorization;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogServices.Interfaces;
using BlogServicesInterfaces;
using BlogDomain.DomainEnums;
using BlogImporterDomain;
using Importers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [ExceptionFilter]
    [AuthenticationFilter]
    [ApiController]
    [Route("api/articles")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService service;
        private readonly IImporterService importerService;

        public ArticleController(IArticleService services, IImporterService importerService)
        {
            this.service = services;
            this.importerService = importerService;
        }

        [HttpPost]
        public IActionResult PostArticle([FromBody] InModelArticle articleIn)
        {
            Article article = articleIn.ToEntity();
            RunWordControl(article);
            Article addedArticle = service.AddArticle(article);
            OutModelArticle articleOut = new OutModelArticle(addedArticle);
            return Ok(articleOut);
        }

        [ArticleVisibilityAuthorizationFilter]
        [HttpGet("{id}")]
        public IActionResult GetArticleById([FromRoute] string id)
        {
            Article requestedArticle = service.GetArticleById(id);
            OutModelArticle articleOut = new OutModelArticle(requestedArticle);
            return Ok(articleOut);
        }

        [ContentAuthorizationFilter]
        [HttpDelete("{id}")]
        public IActionResult DeleteArticleById([FromRoute] string id)
        {
            service.DeleteArticleById(id);
            return Ok();
        }

        [ContentAuthorizationFilter]
        [HttpPut("{id}")]
        public IActionResult UpdateArticle([FromBody] InModelArticle articleIn, [FromRoute] string id)
        {
            Article article = articleIn.ToEntity();
            article.Id = id;
            RunWordControl(article);
            Article articleToEdit = service.UpdateArticle(article);
            OutModelArticle articleOut = new OutModelArticle(articleToEdit);
            return Ok(articleOut);
        }

        [HttpGet]
        public IActionResult GetFeed()
        {
            Article[] articles = service.GetFeed();
            OutModelArticle[] articlesOut = articles.Select(a => new OutModelArticle(a)).ToArray();
            return Ok(articlesOut);
        }

        [HttpGet("importers")]
        public IActionResult GetImporters()
        {
            List<IArticleImporter> importers = importerService.GetArticleImporters();
            List<OutModelImporter> models = importers.Select(i => new OutModelImporter(i)).ToList();
            return Ok(models);
        }

        [HttpPost("import")]
        public IActionResult Import([FromBody] InModelImport import)
        {
            List<Parameter> parameters = import.Parameters.Select(p => p.ToEntity()).ToList();
            List<Article> articles = importerService.ImportArticles(import.Name, parameters);

            foreach(Article article in articles) 
            {
                RunWordControl(article);
                Article addedArticle = service.AddArticle(article);
            }

            return Ok(articles.Select(a => new OutModelArticle(a)).ToList());
        }

        private void RunWordControl(Article article)
        {
            article.WaitingForRevision = false;
            User? user = HttpContext.Items["loggedUser"] as User;
            if (!user.HasRole(UserRole.Admin))
                service.VerifyWords(article);
        }
    }
}
