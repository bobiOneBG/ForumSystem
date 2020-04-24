namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesController : Controller
    {
        private const int ItemsPerPage = 5;

        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public CategoriesController(
            ICategoriesService categoriesService,
            IPostsService postsService)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        public IActionResult ByName(string name, int page = 1)
        {
            var viewModel = this.categoriesService
                .GetByName<CategoryViewModel>(name);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.ForumPosts = this.postsService
                .GetByCategoryId<PostInCategoryViewModel>(
                    viewModel.Id,
                    ItemsPerPage, /*Take() */
                    (page - 1) * ItemsPerPage); /*Skip() */

            var count = this.postsService.GetPostsCountByCategoryId(viewModel.Id);

            viewModel.PagesCount = ((count - 1) / ItemsPerPage) + 1;

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }
    }
}
