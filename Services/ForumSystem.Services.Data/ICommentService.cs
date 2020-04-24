namespace ForumSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task Create(int postId, string content, string userId, int? parentId);

        bool IsInPostId(int commentId, int postId);
    }
}
