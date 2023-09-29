using DeliveryService.API.Services;
using DeliveryService.Context.Context;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly DeliveryServiceContext _context;
        private PlainExceptionsDescriptor _descriptor;
        public PackagesController(DeliveryServiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Package>> CreatePackageAsync(Package package)
        {
            ActionResult result;

            try
            {
                Ticket? ticket = _context.Tickets.FirstOrDefault(c => c.TicketId.Equals(package.TicketId));

                bool isValid = false;

                if (ticket is null)
                    HandleErrorMessage("Ticket with specified id not found.");

                else if (string.IsNullOrEmpty(package.DeliveryAddress))
                    HandleErrorMessage("Delivery address is null or white space.");
                else
                {
                    isValid = true;
                    package.DateCollected = DateTime.UtcNow;
                    package.DateUpdated = DateTime.UtcNow;

                    await _context.Packages.AddAsync(package);
                    await _context.SaveChangesAsync();
                }

                result = isValid ? Ok(package) : BadRequest(_descriptor?.Build());
            }
            catch (Exception exception)
            {
                result = BadRequest(PlainExceptionsDescriptor.Descript(exception));
            }

            return result;
        }

        [HttpPut]
        public async Task<ActionResult<Package>> UpdatePackageAsync(Package package)
        {
            ActionResult result;

            try
            {
                Ticket? ticket = _context.Tickets.FirstOrDefault(c => c.TicketId.Equals(package.TicketId));

                bool isValid = false;

                if (!_context.Packages.Any(x => x.PackageId.Equals(package.PackageId)))
                    HandleErrorMessage("Package with specified id not found.");

                else if (ticket is null)
                    HandleErrorMessage("Ticket with specified id not found.");
                else
                {
                    isValid = true;
                    package.DateUpdated = DateTime.UtcNow;
                    _context.Entry(package).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                result = isValid ? Ok(package) : BadRequest(_descriptor?.Build());
            }
            catch (Exception exception)
            {
                result = BadRequest(PlainExceptionsDescriptor.Descript(exception));
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Package>>> GetPackagesAsync()
        {
            return await _context.Packages.ToListAsync();
        }

        private void HandleErrorMessage(string message)
        {
            _descriptor ??= PlainExceptionsDescriptor.PopulateSingleInstance();
            _descriptor.AppendMessage(message);
        }
    }
}
