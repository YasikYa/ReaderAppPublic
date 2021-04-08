using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NSwag;
using NSwag.Generation.Processors.Security;
using ReaderApp.Authorization;
using ReaderApp.Authorization.Handlers;
using ReaderApp.Commands.Infrastructure;
using ReaderApp.Commands.Infrastructure.Middleware;
using ReaderApp.Data;
using ReaderApp.Data.Exceptions;
using ReaderApp.Services.Abstract;
using ReaderApp.Services.Concrete;
using ReaderApp.Services.Concrete.APIs.Clients;
using ReaderApp.Services.Concrete.WordsFilters;
using ReaderApp.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace ReaderApp
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddDbConnections(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<ReaderAppContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database"), b => b.MigrationsAssembly(typeof(ReaderAppContext).Assembly.FullName)));
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            return services.AddSingleton(config.CreateMapper());
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerDocument(config =>
            {
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT"));
                config.AddSecurity("JWT", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer {access_token}"
                });
            });
        }

        public static IServiceCollection AddSPA(this IServiceCollection services)
        {
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
                {
                    var jwtConfig = configuration.GetSection("JWT").Get<JWTConfigReader>();
                    var signInKey = JWTHelper.CreateTokenSignInKey(jwtConfig.TokenSecurityKey);

                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = jwtConfig.Audience,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = signInKey
                    };
                });

            // Register policies and handlers
            services.AddScoped<IAuthorizationHandler, FilesOperationsAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, UsersOperationsAuthorizationHandler>();

            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddCORS(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }

        public static IServiceCollection AddConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTConfigReader>(configuration.GetSection("JWT"));
            services.AddTransient<IJWTConfig, JWTConfig>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILanguageWordsProvider, LocalLanguageWordsProvider>(factory => new LocalLanguageWordsProvider(Path.Combine(Util.GetExecutingAssemblyDirectory(), "Dictionaries")));
            services.AddSingleton<IUserFileStore, LocalUserFileStore>(factory => new LocalUserFileStore(configuration.GetValue<string>("UserFileStorePath")));
            services.AddSingleton<IFileProcessedWordsCache, LocalProcessedWordsCache>(factory => new LocalProcessedWordsCache(configuration.GetValue<string>("UserFileStorePath")));
            services.AddSingleton<IUserDictionary, LocalUserDictionary>(factory => new LocalUserDictionary(configuration.GetValue<string>("UserFileStorePath")));
            services.AddSingleton<PipeBuilder>();
            services.AddSingleton<ILemmatizer, LemmaSharpAdapter>(factory => new LemmaSharpAdapter(new LemmaSharp.Lemmatizer(File.OpenRead(Path.Combine(Util.GetExecutingAssemblyDirectory(), @"Resources\full7z-multext-en.lem")))));

            return services;
        }

        public static IServiceCollection AddMediatr(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserIdentityPipe<,>));
            return services.AddMediatR(typeof(HandlerBase).Assembly);
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<ITranslationProvider, SystranApiClient>();
            services.AddHttpClient<IWordDefinitionProvider, WordsApiClient>();

            return services;
        }

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    var pathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    object exceptionJson = new { pathFeature?.Error.Message };

                    // if we threw the exception, ensure the response contains the right status code
                    context.Response.StatusCode = pathFeature?.Error is AppExceptionBase ? (int)(pathFeature.Error as AppExceptionBase)?.StatusCode : 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(exceptionJson), context.RequestAborted);
                });
            });
        }
    }
}
