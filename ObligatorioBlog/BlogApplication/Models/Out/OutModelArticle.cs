using BlogDomain;
using BlogDomain.DomainEnums;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BlogApplication.Models.Out
{
    public class OutModelArticle
    {
        public string Owner { set; get; }
        public string Id { set; get; }
        public string Title { set; get; }
        public string Text { set; get; }
        public List<string> Images { set; get; }
        public string Template { set; get; }
        public List<OutModelComment> Comments { set; get; }

        public OutModelArticle(Article article) {
            this.Title = article.Title; 
            this.Text = article.Text;
            this.Owner = article.OwnerUsername;
            this.Id = article.Id;
            this.Images = article.Images.Select(i => i.Content).ToList();
            this.Template = Enum.GetName(typeof(ArticleTemplate), article.Template);
            this.Comments = article.Comments.Select(c => new OutModelComment(c)).ToList();
            
        }

        public override bool Equals(object? obj) => Equals(obj as OutModelArticle);

        public bool Equals(OutModelArticle model) =>
            this.Id == model.Id && 
            this.Owner == model.Owner && 
            this.Title == model.Title && 
            this.Text == model.Text && 
            this.Template == model.Template;
    }
}