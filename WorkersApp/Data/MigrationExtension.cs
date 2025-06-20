using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace WorkersApp.Data;

internal static class MigrationExtension
{
    internal static void ApplyMigration(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();

            if (!dbContext.Departamento.Any() && !dbContext.Provincia.Any() && !dbContext.Distrito.Any())
            {
                var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "Scripts", "inserts.sql");
                if (File.Exists(sqlFilePath))
                {
                    var sql = File.ReadAllText(sqlFilePath);

                    var commands = sql
                        .Split(new[] { "\r\nGO\r\n", "\nGO\n", "\rGO\r", "\r\nGO\n", "\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    using var transaction = dbContext.Database.BeginTransaction();
                    try
                    {
                        foreach (var command in commands)
                        {
                            var trimmed = command.Trim();
                            if (!string.IsNullOrWhiteSpace(trimmed))
                            {
                                dbContext.Database.ExecuteSqlRaw(trimmed);
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
