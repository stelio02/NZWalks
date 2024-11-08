using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{

    // https://localhost/7118/api/students 

    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            string[] studentNames = new string[] { "Stelio", "Eri", "Egi" };

            return Ok(studentNames);
        }

    }
}
