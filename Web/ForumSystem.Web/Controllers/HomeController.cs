namespace ForumSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using ForumSystem.Data;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        // private readonly ApplicationDbContext dbContext;
        // private readonly IDeletableEntityRepository<Category> categoriesRepository;
        public HomeController(ICategoriesService categoriesService)
        /*ApplicationDbContext dbContext,
         IDeletableEntityRepository<Category> repository*/
        {
            this.categoriesService = categoriesService;

            // this.dbContext = dbContext;
            // this.categoriesRepository = repository;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Categories =
                this.categoriesService.GetAll<IndexCategoryViewModel>(),
            };

            // var categories = /*this.dbContext.Categories*/
            //    this.categoriesService.GetAll<IndexCategoryViewModel>();

            // IndexCategoryViewModel -> :IMapFrom<Category>
            // .To<IndexCategoryViewModel>()

            // .Select(x => new IndexCategoryViewModel
            // {
            //     Name = x.Name,
            //     Title = x.Title,
            //     Description = x.Description,
            //     ImageUrl = x.ImageUrl,
            // })
            // .ToList();
            // viewModel.Categories = categories;
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
