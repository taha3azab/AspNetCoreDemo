using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Identity
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you wan to add an MVC-based UI
            //services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

            if (_environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to support static files
            //app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseHttpsRedirection();
            app.UseIdentityServer();

            // uncomment, if you wan to add an MVC-based UI
            //app.UseMvcWithDefaultRoute();
        }
    }
}