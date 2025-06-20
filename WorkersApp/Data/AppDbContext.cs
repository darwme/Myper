using Microsoft.EntityFrameworkCore;
using WorkersApp.Models;

namespace WorkersApp.Data;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Trabajador> Trabajadores { get; set; } = null!;
    public DbSet<Departamento> Departamento { get; set; } = null!;
    public DbSet<Provincia> Provincia { get; set; } = null!;
    public DbSet<Distrito> Distrito { get; set; } = null!;
}
