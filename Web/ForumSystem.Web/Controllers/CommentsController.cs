namespace ForumSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Comments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCommentsInputModel input)
        {
            var parentId = input.ParentId == 0
                ? null :
                input.ParentId;

            // defense from users foolishness
            if (parentId.HasValue)
            {
                if (!this.commentService.IsInPostId(parentId.Value, input.PostId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.commentService.Create(input.PostId, input.Content, userId, parentId);

            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }
    }
}
