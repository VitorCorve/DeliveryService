using DeliveryService.API.Services;
using DeliveryService.Context.Context;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouriersController : ControllerBase
    {
        private readonly DeliveryServiceContext _context;
        public CouriersController(DeliveryServiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Courier>> CreateCourierAsync(Courier courier)
        {
            ActionResult result;

            try
            {
                await _context.Couriers.AddAsync(courier);
                await _context.SaveChangesAsync();
                result = Ok(courier);
            }
            catch (Exception exception)
            {
                result = BadRequest(PlainExceptionsDescriptor.Descript(exception));
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Courier>>> GetCouriersAsync()
        {
            return await _context.Couriers.ToListAsync();
        }
    }
}
