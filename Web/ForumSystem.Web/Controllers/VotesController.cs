namespace ForumSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        // POST -> /api/votes
        // Request body {"postId:3 , "isUpVote" : true"}
        // Response body {"votesCount : 11"}
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<VoteResponseModel>> Post(VoteInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.votesService.VoteAsync(input.PostId, userId, input.IsUpVote);

            var votes = this.votesService.GetVotes(input.PostId);

            return new VoteResponseModel { VotesCount = votes };
        }
    }
}
