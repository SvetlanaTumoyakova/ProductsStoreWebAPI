using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ProductsStore.DAL;
using ProductsStore.WebAPI.Providers;
using ProductsStore.WebAPI.Providers.Interface;
using ProductsStore.WebAPI.Service;
using ProductsStore.WebAPI.Service.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IAuthProvider, AuthProvider>();
builder.Services.AddScoped<ProductProvider>();
builder.Services.AddScoped<CartProvider>();
builder.Services.AddScoped<OrderProvider>();

builder.Services.AddControllers();


builder.Services.AddDbContext<DataBaseContext>(options => { 
    options.UseNpgsql(builder.Configuration.GetConnectionString("TestConnection"));
    options.UseSnakeCaseNamingConvention();
    });


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowLocalhost", builder => {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhost");

var staticFolder = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "static");
if (!Directory.Exists(staticFolder))
{
    Directory.CreateDirectory(staticFolder);
}
app.UseStaticFiles(); // serves wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFolder),
    RequestPath = "/static"
});

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
