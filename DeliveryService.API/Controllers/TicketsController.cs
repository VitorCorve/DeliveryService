using DeliveryService.Context.Context;
using DeliveryService.Context.Domain;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly DeliveryServiceContext _context;
        public TicketsController(DeliveryServiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicketAsync(int customerId, int[] packageIds)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsAsync(int? customerId, int? courierId, int? packageId, string? searchQuery)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<Ticket>> EditTicketAsync(int tickedId, DELIVERY_STATUS status, string? commentary)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTicketAsync(int tickedId)
        {
            throw new NotImplementedException();
        }
    }
}
