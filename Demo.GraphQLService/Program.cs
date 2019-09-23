using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Demo.GraphQLService
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Run();


        public static IWebHost CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseKestrel(options =>
                    {
                        options.AddServerHeader = false;
                    })
                    .UseStartup<Startup>()
                    .UseSerilog((context, config) =>
                    {
                        config.WriteTo.File(@"graphql_log.txt")
                            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                    }).Build();
    }
}
