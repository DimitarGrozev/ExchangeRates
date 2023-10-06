using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeRates.Data.Models
{
    public class RequestHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid  RequestId { get; set; }

        public string ClientId { get; set; }

        public string ExitService { get; set; }

        public long Timestamp { get; set; }

        public string Currency { get; set; }
    }
}
