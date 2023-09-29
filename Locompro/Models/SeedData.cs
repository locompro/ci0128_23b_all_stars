using Microsoft.EntityFrameworkCore;
using Locompro.Data;

namespace Locompro.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LocomproContext(
           serviceProvider.GetRequiredService<
               DbContextOptions<LocomproContext>>()))
            {
                if (!context.Country.Any())
                {
                    context.Country.AddRange(
                        new Country
                        {
                            Name = "Costa Rica"
                        }
                        );
                }

                if (!context.Province.Any())
                {
                    context.Province.AddRange(
                       new Province
                       {
                           Name = "San José",
                           Country = context.Country.Find("Costa Rica")
                       }
                   );
                }

                if (context.Province.Find("Alajuela") == null)
                {
                    context.Province.AddRange(
                        new Province
                        {
                            Name = "Alajuela",
                            Country = context.Country.Find("Costa Rica")
                        }
                    );
                }

                if (!context.Canton.Any())
                {
                    context.Canton.AddRange(
                        new Canton
                        {
                            Province = context.Province.Find("San José"),
                            Name = "San José"
                        },

                        new Canton
                        {
                            Province = context.Province.Find("San José"),
                            Name = "Tibas"
                        },

                        new Canton
                        {
                            Province = context.Province.Find("San José"),
                            Name = "Desamparados"
                        }
                    );
                }

                context.SaveChanges();
            }
        }
    }
}
