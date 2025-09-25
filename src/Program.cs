using TodosApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
            // If you send cookies/Authorization from the browser, also add:
            // .AllowCredentials();
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

// app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("FrontendDev");
app.MapControllers();
app.Run();
