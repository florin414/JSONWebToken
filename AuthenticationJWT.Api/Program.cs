var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddTransient<AuthInterceptor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddAuthenticationJwt(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddConfig(builder.Configuration);
builder.Services.AddHttpClient();

builder.Configuration.AddAmazonSystemsManager(builder.Environment);
builder.Configuration.AddAmazonSecretsManager(builder.Environment);

builder.Services.AddDbSqlLite(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSpoonacularFoodServices();

var app = builder.Build();

app.UseMiddleware<AuthInterceptor>();
app.UseCors("AllowAngularClient");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseSwaggerIfDevelopment();
app.UseRouting();

app.UseJwtMiddlware();
app.UseMiddleware<ExceptionMiddleware>();
//app.UseErrorHandlerMiddlware();

app.MapAuthEndpointsApi();
app.MapSpoonacularFoodEndpointsApi();
app.Services.DbSqlLiteMigrate();

app.Run();
