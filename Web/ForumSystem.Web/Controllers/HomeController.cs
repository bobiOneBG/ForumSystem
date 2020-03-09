namespace ForumSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using ForumSystem.Data;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();

            var categories = this.dbContext.Categories.Select(x => new IndexCategoryViewModel
            {
                Name = x.Name,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
            }).ToList();

            viewModel.Categories = categories;

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
