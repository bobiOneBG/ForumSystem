namespace ForumSystem.Web.ViewModels.Comments
{
    public class CreateCommentsInputModel
    {
        public int PostId { get; set; }

        public int? ParentId { get; set; }

        public string Content { get; set; }
    }
}
