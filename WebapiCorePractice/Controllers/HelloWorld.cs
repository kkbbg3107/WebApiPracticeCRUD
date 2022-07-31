using Microsoft.AspNetCore.Mvc;

namespace WebapiCorePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorld : Controller
    {
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id.ToString()+ "Test";
        }

        [HttpGet("hi")]
        public IEnumerable<string> GetAll()
        {
            var list = new List<string>()
            {
                "kane", "vera", "Tom"
            };
            return list; 
        }
    }
}
