using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Cafe.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args, null).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, params string[] urls)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

            if (urls?.Length > 0)
            {
                builder.UseUrls(urls);
            }

            return builder;
        }
    }
}
