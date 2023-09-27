using DeliveryService.Context.Context;
using DeliveryService.Context.Models;
using Microsoft.AspNetCore.Mvc;

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
            throw new NotImplementedException();
        }
    }
}
