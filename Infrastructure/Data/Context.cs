using Microsoft.EntityFrameworkCore;
using InputWeb.Domain.Entities;

namespace InputWeb.Infrastructure.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<Recording> Recordings => Set<Recording>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}