using DeliveryService.Context.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Context.Models
{
    public class Ticket
    {
        public Ticket()
        {
            Packages = new HashSet<Package>();
        }

        [Key]
        public int TicketId { get; set; }
        public int CustomerId { get; set; }
        public int? CourierId { get; set; }
        public DELIVERY_STATUS Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Completed { get; set; }
        public string? Commentary { get; set; }
        public bool IsClosed { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Tickets")]
        public Customer? Customer { get; set; }

        [ForeignKey(nameof(CourierId))]
        [InverseProperty("Tickets")]
        public Courier? Courier { get; set; }

        [InverseProperty(nameof(Package.Ticket))]
        public IEnumerable<Package> Packages { get; set; }

    }
}
