using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Infrastructure.Data;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _storeContext;

        public BuggyController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        [HttpGet("notfound")] // GET: /api/Buggy/notfound
        public ActionResult GetNotFoundError()
        {
            var product = _storeContext.Products.Find(1000);
            if(product is null)
                return NotFound();
            return Ok(product);
        }
        
        [HttpGet("servererror")] // GET: /api/Buggy/servererror
        public ActionResult GetSeverError()
        {
            var product = _storeContext.Products.Find(1000);
            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }
        
        [HttpGet("badrequest")] // GET:/api/Buggy/badrequest
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
        
        [HttpGet("badrequest/{id}")] // GET: /api/Buggy/badrequest/five
        public ActionResult GetBadRequest(int id) // Validation Error
        {
            return Ok();
        }
        
        [HttpGet("unauthorized")] // GET: /api/Buggy/unauthorized
        public ActionResult GetUnAuthorizedError()
        {
            return Unauthorized();  
        }
        
        

    }
}
