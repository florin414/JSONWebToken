using AuthenticationJWT.Api.Extensions.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationJwt(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddConfig(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddDbSqlLite(builder.Configuration);

builder.Configuration.AddAmazonSystemsManager(builder.Environment);
builder.Configuration.AddAmazonSecretsManager(builder.Environment);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSpoonacularFoodServices();

var app = builder.Build();

app.UseSwaggerIfDevelopment();
app.UseHttpsRedirection();
app.UseRouting();

app.UseJwtMiddlware();
app.UseAuthentication();
app.UseAuthorization();
app.Services.DbSqlLiteMigrate();

app.MapAuthEndpointsApi();
app.MapSpoonacularFoodEndpointsApi();

app.Run();
