using DeliveryService.Context.Context;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly DeliveryServiceContext _context;
        public PackagesController(DeliveryServiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Package>> CreatePackageAsync(Package package, int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
