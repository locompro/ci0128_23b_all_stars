using Microsoft.EntityFrameworkCore;
using Locompro.Data;
using Locompro.Models;

namespace Locompro.Models
{
    public static class SeedData
    {
        public static void Initialize(LocomproContext context)
        {

            bool isInitialized = context.Country.Any() &&
                                    context.Province.Any() &&
                                    context.Canton.Any() &&
                                    context.Category.Any() &&
                                    context.Submission.Any() &&
                                    context.Store.Any() &&
                                    context.Product.Any();

            // Check if the database is initialized
            if (isInitialized)
            {
                // Database has been seeded
                return;
            }

            // Read SQL script
            string sqlScript = File.ReadAllText("Services/Resources/static.sql");

            // Execute SQL script
            context.Database.ExecuteSqlRaw(sqlScript);

            // Read SQL script
            sqlScript = File.ReadAllText("Services/Resources/dummy.sql");

            // Execute SQL script
            context.Database.ExecuteSqlRaw(sqlScript);
        }
    }
}
