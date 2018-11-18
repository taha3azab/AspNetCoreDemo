using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Demo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel((context, options) =>
                {
                    options.AddServerHeader = false;
                    options.Limits.MaxConcurrentConnections = 100;
                });
    }
}
