namespace ForumSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;

        public PostsController(IPostsService postsService, ICategoriesService categoriesService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
        }

        public IActionResult ById(int id)
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Create()
        {
            var categories = this.categoriesService
                .GetAll<CategoryDropDownViewModel>();

            var viewModel = new PostCreateInputModel
            {
                Categories = categories,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postId = await this.postsService.CreateAsync(input.Title, input.Content, input.CategoryId, userId);

            return this.RedirectToAction(nameof(this.ById), new { postId });
        }
    }
}
