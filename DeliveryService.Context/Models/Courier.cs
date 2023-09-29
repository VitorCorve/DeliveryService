using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Context.Models
{
    public class Courier
    {
        public Courier()
        {
            Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int CourierId { get; set; }

        [InverseProperty(nameof(Ticket.Courier))]
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
