namespace ForumSystem.Web.ViewModels.Categories
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class PostInCategoryViewModel : IMapFrom<Post>
    {
        public string Title { get; set; }

        public string UserUserName { get; set; }

        public int CommentsCount { get; set; }
    }
}
