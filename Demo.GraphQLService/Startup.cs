using Demo.GraphQLService.Data;
using Demo.GraphQLService.GraphQL;
using Demo.GraphQLService.Services;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.GraphQLService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<LibraryContext>(options =>
                    //options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"))
                    options.UseInMemoryDatabase("LibraryDB")
                ).AddUnitOfWork<LibraryContext>();

            services.AddTransient<Seed>();

            services.AddScoped<ILibraryService, LibraryService>();

            services.AddScoped<IDependencyResolver>(_ => new FuncDependencyResolver(_.GetRequiredService));
            services.AddScoped<LibrarySchema>();
            services.AddSingleton<LibraryMessageService>();

            services.AddGraphQL(_ =>
                {
                    _.EnableMetrics = true;
                    _.ExposeExceptions = true;
                })
            .AddGraphTypes(ServiceLifetime.Scoped)
            .AddDataLoader()
            .AddWebSockets();

            services.AddCors();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                seeder.SeedLibrary().Wait();
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseWebSockets();

            // add http for Schema at default url /graphql
            app.UseGraphQLWebSockets<LibrarySchema>("/graphql");
            app.UseGraphQL<LibrarySchema>("/graphql");

            // use graphql-playground at default url /ui/playground
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground"
            });

        }
    }
}
