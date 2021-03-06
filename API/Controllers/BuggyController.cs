using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        public StoreContext _context { get; }
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundError()
        {
            var thing = _context.Products.Find(4534);
            if(thing == null){
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

         [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(4534);
            var thing_ = thing.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {            
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest2(int id)
        {            
            return Ok();
        }

        [HttpGet("testAuth")]
        [Authorize]
        public ActionResult<string> GetSecretKey(){
            return "secret key";
        }
    }
}