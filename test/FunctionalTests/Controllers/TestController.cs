using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionalTests.Controllers
{
    [Route("api/[controller]")]
    public class TestController: ControllerBase
    {

        [Authorize]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
