using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CacheManager.Core;
using Demo.API.Data;
using Demo.API.Dtos;
using Demo.API.Helpers;
using Demo.API.Models;
using EFSecondLevelCache.Core;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Demo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")))
                    .AddUnitOfWork<DataContext>();

            //services.AddEFSecondLevelCache();

            // Add an in-memory cache service provider
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new CacheManager.Core.ConfigurationBuilder()
                        .WithJsonSerializer()
                        .WithMicrosoftMemoryCacheHandle()
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                        .Build());
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                // options.MimeTypes =
                //     ResponseCompressionDefaults.MimeTypes.Concat(
                //         new[] { "application/xml", "application/json", "image/svg+xml" });
                options.EnableForHttps = true;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddResponseCaching();
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .AddXmlSerializerFormatters()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            services.AddCors();
            services.AddTransient<Seed>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Add API Versioning
            // the default version is 1.0
            // and we're going to read the version number from the media type
            // incoming requests should have a accept header like this: Accept: application/json;v=1.0
            services.AddApiVersioning(o =>
            {
                //o.ApiVersionReader = new HeaderApiVersionReader("api-version");UrlSegmentApiVersionReader
                o.DefaultApiVersion = new ApiVersion(2, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new MediaTypeApiVersionReader();
                o.ReportApiVersions = true;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2.0", new Info { Title = "Versioned Api v2.0", Version = "v2.0" });
                c.SwaggerDoc("v1.0", new Info { Title = "Versioned Api v1.0", Description = "Deprecated", Version = "v1.0" });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    // would mean this action is unversioned and should be included everywhere
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);

                });
                c.OperationFilter<ApiVersionOperationFilter>();
                c.OperationFilter<AddAuthTokenHeaderParameter>();
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });

                app.UseHsts();
            }
            // seeder.SeedUsers();
            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.EnableDeepLinking();
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned Api v1.0");
                c.SwaggerEndpoint("/swagger/v2.0/swagger.json", "Versioned Api v2.0");
                c.DocExpansion(DocExpansion.None);
            });
            app.UseAuthentication();
            app.UseResponseCompression();
            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                // For GetTypedHeaders, add: using Microsoft.AspNetCore.Http;
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            TypeAdapterConfig<User, UserForListDto>
                            .NewConfig()
                            .IgnoreNullValues(true)
                            .AvoidInlineMapping(true)
                            .Map(dest => dest.Age, src => src.DateOfBirth.CalculateAge())
                            .Map(dest => dest.PhotoUrl, src => src.Photos.FirstOrDefault(p => p.IsMain).Url)
                            .IgnoreIf((src, dest) => src.Photos.FirstOrDefault(p => p.IsMain) == null, dest => dest.PhotoUrl);
            TypeAdapterConfig<User, UserForDetailedDto>
                            .NewConfig()
                            .IgnoreNullValues(true)
                            .AvoidInlineMapping(true)
                            .Map(dest => dest.Age, src => src.DateOfBirth.CalculateAge())
                            .Map(dest => dest.PhotoUrl, src => src.Photos.FirstOrDefault(p => p.IsMain).Url)
                            .IgnoreIf((src, dest) => src.Photos.FirstOrDefault(p => p.IsMain) == null, dest => dest.PhotoUrl);
            app.UseMvc();
        }
    }
}
