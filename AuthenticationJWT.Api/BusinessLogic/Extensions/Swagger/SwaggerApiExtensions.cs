namespace AuthenticationJWT.Api.BusinessLogic.Extensions.Swagger;

public static class SwaggerApiExtensions
{
    public static IApplicationBuilder UseSwaggerIfDevelopment(this IApplicationBuilder app)
    {
        if (
            app.ApplicationServices.GetService(typeof(IWebHostEnvironment))
                is IWebHostEnvironment env
            && env.IsDevelopment()
        )
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpoonacularFoodApi")
            );
        }

        return app;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpoonacularFoodApi", Version = "v1" });
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            );
        });
        return services;
    }
}
