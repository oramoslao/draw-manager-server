using AutoMapper;
using DrawManager.Api;
using DrawManager.Database.SqlServer;
using DrawManager.Domain.Extensions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using DomainConstants = DrawManager.Domain.Constants;
using Infrastructure = DrawManager.Api.Infrastructure;

namespace DrawManager
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = ApplicationConfiguration.GetConfiguration(Directory.GetCurrentDirectory(), env.EnvironmentName);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registering MediatR options
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.ValidationPipelineBehavior<,>));

            // Registering database context
            //var dbConnString = Environment.GetEnvironmentVariable(DrawManagerApiWellKnownConstants.DB_CONNECTIONSTRING_KEY)
            //    ?? Configuration.GetConnectionString(DrawManagerApiWellKnownConstants.DB_CONNECTIONSTRING_KEY);

            //services
            //    .AddDbContext<DrawManagerDbContext>(options => options.UseSqlite(dbConnString));

            var connectionString = Configuration.GetConnectionString(DomainConstants.CONNECTION_STRING_NAME_SQL_SERVER);
            services.AddDbContext<DrawManagerSqlServerDbContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(DomainConstants.SQL_SERVER_ENTITY_FRAMEWORK_ASSEMBLY_NAME)));

            // Registering Cors
            services.AddCors();

            // Registering AutoMapper
            services.AddAutoMapper(GetType().Assembly);

            // Registering services
            services.AddScoped<Infrastructure.IRandomSelector, Infrastructure.RandomSelector>();
            services.AddScoped<Infrastructure.IPasswordHasher, Infrastructure.PasswordHasher>();
            services.AddScoped<Infrastructure.IJwtTokenGenerator, Infrastructure.JwtTokenGenerator>();
            services.AddScoped<Infrastructure.ICurrentUserAccessor, Infrastructure.CurrentUserAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Registering Jwt
            services.AddJwt();

            // Registering Mvc options
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(Infrastructure.ValidatorActionFilter));
                options.EnableEndpointRouting = false;
            }).AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            //// Registering swagger options
            var swaggerVersion = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_VERSION_KEY];
            var swaggerTitle = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_TITLE_KEY];

            services.AddSwagger(swaggerVersion, swaggerTitle);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Enable database creation ensuring and migrations
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<DrawManagerSqlServerDbContext>().Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Serilog
            loggerFactory.AddSerilogLogging();

            // Enable error's middleware
            app.UseMiddleware<Infrastructure.ErrorHandlingMiddleware>();

            // Enable Cors
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseAuthentication();

            // Enable Mvc
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
            var swaggerUrl = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_ENDPOINT_KEY];
            var swaggerName = Configuration[DrawManagerApiWellKnownConstants.SWAGGER_NAME_KEY];
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerUrl, swaggerName);
            });

        }
    }
}
