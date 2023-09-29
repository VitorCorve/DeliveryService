using DeliveryService.API.Services;
using DeliveryService.Context.Context;
using DeliveryService.Context.Domain;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<Ticket>> CreateTicketAsync(int customerId)
        {
            ActionResult result;

            try
            {
                Customer? customer = _context.Customers.FirstOrDefault(c => c.CustomerId.Equals(customerId));
                if (customer is null)
                {
                    result = BadRequest("Customer with specified id isn't exists.");
                }
                else
                {

                    Ticket ticket = new()
                    {
                        CustomerId = customerId,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow
                    };

                    await _context.Tickets.AddAsync(ticket);
                    await _context.SaveChangesAsync();

                    result = Ok(ticket);
                }
            }
            catch (Exception exception)
            {
                result = BadRequest(PlainExceptionsDescriptor.Descript(exception));
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsAsync(int? customerId, int? courierId, bool? isClosed, DELIVERY_STATUS? status, string? searchQuery, DateTime? dateFrom, DateTime? dateTo)
        {
            List<Ticket> result = await _context.Tickets
                .Include(c => c.Courier)
                .Include(c => c.Customer)
                .ToListAsync();

            if (isClosed != null)
                result = await _context.Tickets.Where(t => t.IsClosed.Equals(isClosed)).ToListAsync();

            if (status != null)
                result = await _context.Tickets.Where(t => t.Status.Equals(status)).ToListAsync();

            if (customerId != null)
                result = await _context.Tickets.Where(t => t.CustomerId.Equals(customerId)).ToListAsync();

            if (courierId != null)
                result.AddRange(await _context.Tickets.Where(t => t.Equals(courierId)).ToListAsync());

            if (dateFrom != null)
                result.AddRange(await _context.Tickets.Where(t => t.Created >= dateFrom).ToListAsync());

            if (dateTo != null)
                result.AddRange(await _context.Tickets.Where(t => t.Created <= dateTo).ToListAsync());

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                result = result.Where(x => x.TicketId.ToString().Contains(searchQuery) ||
                    x.Commentary !=null && x.Commentary.ToLower().Contains(searchQuery) ||
                    x.Customer != null && x.Customer.CustomerName.ToLower().Contains(searchQuery) ||
                    x.Status.ToString().ToLower().Contains(searchQuery) ||
                    x.Created.ToString().Contains(searchQuery) ||
                    x.Updated.ToString().Contains(searchQuery))
                    .ToList();
            }

            return result.DistinctBy(r => r.TicketId).ToList();
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<Ticket>> EditTicketAsync(int ticketId, DELIVERY_STATUS status, string? commentary)
        {
            ActionResult result;

            Ticket? ticket = _context.Tickets.FirstOrDefault(t => t.TicketId.Equals(ticketId));

            if (ticket != null)
            {
                switch (status)
                {
                    case DELIVERY_STATUS.New:
                        ticket.IsClosed = false;
                        break;
                    case DELIVERY_STATUS.Processing:
                        break;
                    case DELIVERY_STATUS.Completed:
                        ticket.IsClosed = true;
                        break;
                    case DELIVERY_STATUS.Rejected:
                        if (!string.IsNullOrEmpty(commentary))
                            ticket.Commentary = commentary;
                        break;
                    default:
                        break;
                }

                ticket.Status = status;
                ticket.Updated = DateTime.UtcNow;

                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = Ok(ticket);
            }
            else
                result = BadRequest("Ticked with specified id isn't exists");

            return result;
        }

        [HttpPut("Proccess")]
        public async Task<ActionResult> MoveToProcessingAsync(int ticketId, int courierId)
        {
            ActionResult result;

            Ticket? ticket = _context.Tickets.FirstOrDefault(t => t.TicketId.Equals(ticketId));

            if (ticket != null)
            {
                ticket.Status = DELIVERY_STATUS.Processing;
                ticket.CourierId = courierId;
                ticket.Updated = DateTime.UtcNow;

                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = Ok(ticket);
            }
            else
                result = BadRequest("Ticked with specified id isn't exists");

            return result;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTicketAsync(int ticketId)
        {
            ActionResult result;

            Ticket? ticket = _context.Tickets.FirstOrDefault(t => t.TicketId.Equals(ticketId));

            if (ticket != null)
            {
                var packages = await _context.Packages.Where(p => p.TicketId.Equals(ticketId)).ToListAsync();
                _context.RemoveRange(packages);
                _context.Remove(ticket);
                await _context.SaveChangesAsync();

                result = Ok(ticket);
            }
            else
                result = BadRequest("Ticked with specified id isn't exists");

            return result;
        }
    }
}
