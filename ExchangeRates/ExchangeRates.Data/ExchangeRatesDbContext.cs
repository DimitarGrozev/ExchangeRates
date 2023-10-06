using ExchangeRates.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Data
{
    public class ExchangeRatesDbContext: DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        public DbSet<RequestHistory> RequestHistory{ get; set; }

        public ExchangeRatesDbContext(DbContextOptions<ExchangeRatesDbContext> options) : base(options)
        {
        }
    }
}
