using Microsoft.EntityFrameworkCore;
using TodosApp.Features.Todos;

namespace TodosApp.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Todo>().Property(c => c.Title).HasMaxLength(200).IsRequired();
    }
}
