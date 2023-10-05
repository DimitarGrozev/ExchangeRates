using ExchangeRates.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Data
{
    public class ExchangeRatesDbContext: DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        public ExchangeRatesDbContext(DbContextOptions<ExchangeRatesDbContext> options) : base(options)
        {
        }
    }
}
