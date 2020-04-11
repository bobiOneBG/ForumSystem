namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Linq;

    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Ganss.XSS;

    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitazedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string UserUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VotesCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Post, PostViewModel>()
                    .ForMember(x => x.VotesCount, opts =>
                         {
                             opts.MapFrom(p => p.Votes.Sum(v => (int)v.Type));
                         });
        }
    }
}
