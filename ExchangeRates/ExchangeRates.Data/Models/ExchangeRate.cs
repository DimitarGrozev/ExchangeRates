using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeRates.Data.Models
{
    public class ExchangeRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? Timestamp { get; set; }

        public string? Base { get; set; }

        public DateTime? Date { get; set; }

        public string RatesJson { get; set; }
    }
}
