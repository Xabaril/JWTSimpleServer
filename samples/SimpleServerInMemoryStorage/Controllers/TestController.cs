using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleServerInMemoryStorage.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : Controller
    {
        public IActionResult Get()
        {
            return Ok("Answer from controller : Authorization successful");
        }
    }
}
