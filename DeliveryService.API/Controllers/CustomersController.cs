using DeliveryService.API.Services;
using DeliveryService.Context.Context;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            ActionResult result;

            try
            {
                if (string.IsNullOrWhiteSpace(customer.CustomerName))
                {
                    result = BadRequest("Customer name is null or white space.");
                }
                else
                {
                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();
                    result = Ok(customer);
                }
            }
            catch (Exception exception)
            {
                result = BadRequest(PlainExceptionsDescriptor.Descript(exception));
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }
    }
}
