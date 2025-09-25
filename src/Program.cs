using TodosApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")   // your Next.js dev origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            // add the next line ONLY if you use cookies/auth headers
            .AllowCredentials();
    });
});

builder.Services.AddControllers();

// EF Core + Npgsql
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    opt.UseNpgsql(cs);
});

// Add services to the container.
// Optional: expose minimal OpenAPI for quick testing (Swagger UI is default in dev template)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
