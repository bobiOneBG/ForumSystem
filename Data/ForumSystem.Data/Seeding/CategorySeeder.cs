namespace ForumSystem.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;

    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = new List<(string Name, string ImgUrl)>
            {
                ("Sport", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Falnadialburhani.com%2Fwp-content%2Fuploads%2F2017%2F09%2Fballs.jpg&f=1&nofb=1"),
                ("News", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse3.mm.bing.net%2Fth%3Fid%3DOIP.mBzmf-vow2ve4EBBh4HBlQHaEv%26pid%3DApi&f=1"),
                ("Music", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.ytimg.com%2Fvi%2F_mEQI32SIXY%2Fmaxresdefault.jpg&f=1&nofb=1"),
                ("Programming", "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fnews.efinancialcareers.com%2Fbinaries%2Fcontent%2Fgallery%2Fefinancial-careers%2Farticles%2F2019%2F03%2Fprogrammer.jpg&f=1&nofb=1"),
                ("Cats", "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2F2.bp.blogspot.com%2F-WtdFq_e6eKo%2FTV5W5s-hS-I%2FAAAAAAAAAvM%2FgmCUYOx3bX8%2Fs1600%2FAnimals_Cats_Small_cat_005241_.jpg&f=1&nofb=1"),
                ("Dogs", "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fcdn.cnn.com%2Fcnnnext%2Fdam%2Fassets%2F191114120109-dog-aging-project-1-super-tease.jpg&f=1&nofb=1"),
            };

            foreach (var (name, imgUrl) in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Name = name,
                    Title = name,
                    Description = name,
                    ImageUrl = imgUrl,
                });
            }
        }
    }
}
