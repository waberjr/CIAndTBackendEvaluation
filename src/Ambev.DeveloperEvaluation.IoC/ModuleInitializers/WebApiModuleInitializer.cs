using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers
{
    public class WebApiModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();

            builder.Services.AddOpenApiDocument((configure, sp) =>
            {
                configure.Title = "Ambev Developer Evaluation API - Environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                configure.AddSecurity("JWT", [], new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Type into the textbox your token (Without 'Bearer ')."
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
        }
    }
}
