using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;

namespace Demo.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.SetBasePath(context.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                            .AddJsonFile("ocelot.json", false, true)
                            .AddEnvironmentVariables();
                    })
                    .UseSerilog((context, config) =>
                    {
                        config
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.File(@"ocelot_log.txt")
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                    })
                    .UseKestrel()
                    .UseStartup<Startup>();
    }
}
