using Microsoft.EntityFrameworkCore;
using Locompro.Data;
using Locompro.Models;

namespace Locompro.Models
{
    public static class SeedData
    {
        public static void Initialize(LocomproContext context)
        {

            bool isInitialized = context.Countries.Any() &&
                                    context.Provinces.Any() &&
                                    context.Cantons.Any();

            // Check if the database is initialized
            if (isInitialized)
            {
                // Database has been seeded
                return;
            }

            // Read SQL script
            string sqlScript = File.ReadAllText("./Resources/static.sql");

            // Execute SQL script
            context.Database.ExecuteSqlRaw(sqlScript);

            // Read SQL script
            sqlScript = File.ReadAllText("./Resources/dummy.sql");

            // Execute SQL script
            context.Database.ExecuteSqlRaw(sqlScript);
        }
    }
}
