namespace ForumSystem.Services.Tests
{
    using ForumSystem.Web;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class HomePageTests
    {
        [Fact]
        public async Task HomePageShouldHaveH1Title()
        {
            var serverFactory = new WebApplicationFactory<Startup>();

            var client = serverFactory.CreateClient();

            var response = await client.GetAsync("/");
            var reaponseAsString = await response.Content.ReadAsStringAsync();

            Assert.Contains("<h1",  reaponseAsString);
        }
    }
}
