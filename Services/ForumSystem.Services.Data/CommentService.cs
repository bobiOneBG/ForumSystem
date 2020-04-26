namespace ForumSystem.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CommentService : ICommentService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task Create(int postId, string content, string userId, int? parentId = null)
        {
            var comment = new Comment
            {
                PostId = postId,
                Content = content,
                UserId = userId,
                ParentId = parentId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public bool IsInPostId(int commentId, int postId)
        {
            var commentPostId = this.commentsRepository
                .All()
                .Where(x => x.Id == commentId)
                .Select(x => x.PostId)
                .FirstOrDefault();
            var sss = commentPostId == postId;

            return commentPostId == postId;
        }
    }
}
