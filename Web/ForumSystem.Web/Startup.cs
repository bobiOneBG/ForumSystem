namespace ForumSystem.Web
{
    using System.Reflection;

    using ForumSystem.Data;
    using ForumSystem.Data.Common;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Repositories;
    using ForumSystem.Data.Seeding;
    using ForumSystem.Services.Data;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Services.Messaging;
    using ForumSystem.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(opts =>
            {
                opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                // options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN";

                // options.SuppressXFrameOptionsHeader = false;
            });

            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = this.configuration
                    .GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "CacheRecords";
            });

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services    TO DO ...Sendgrid emailsender
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender("TO DO"));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<IVotesService, VotesService>();
            services.AddTransient<ICommentService, CommentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            "forumCategory",
                            pattern: "f/{name:minlength(3)}",
                            new { controller = "Categories", action = "ByName" });

                        endpoints.MapControllerRoute(
                            "areaRoute",
                            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                        endpoints.MapControllerRoute(
                            "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
