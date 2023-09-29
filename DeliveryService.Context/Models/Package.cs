using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Context.Models
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }
        public int TicketId { get; set; }
        public DateTime DateCollected { get; set; }
        public DateTime? DateDelivered { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime RequestedDeliveryDate { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public string GatheringAddress { get; set; } = string.Empty;

        [ForeignKey(nameof(TicketId))]
        [InverseProperty("Packages")]
        public Ticket? Ticket { get; set; }
    }
}
