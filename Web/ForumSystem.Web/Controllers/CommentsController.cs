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
        private readonly ICommentService commentsService;

        public CommentsController(ICommentService commentsService)
        {
            this.commentsService = commentsService;
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
                if (!this.commentsService.IsInPostId(parentId.Value, input.PostId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.commentsService.Create(input.PostId, input.Content, userId, parentId);

            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }
    }
}
