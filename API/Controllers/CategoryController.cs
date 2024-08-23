using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {

        public CategoryController() {

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost() {

            return Ok();
        }
    }
}
