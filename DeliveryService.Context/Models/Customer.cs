using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Context.Models
{
    public class Customer
    {
        public Customer()
        {
            Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        [InverseProperty(nameof(Ticket.Customer))]
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
