using DeliveryService.Context.Context;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly DeliveryServiceContext _context;
        public CustomersController(DeliveryServiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
