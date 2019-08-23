using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloWorldConsole.BLL;
using HelloWorldConsole.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelloWorldConsole.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private Context context = null;
        public ILogger<HelloController> Logger { get; }
        
        public HelloController(IConfiguration config, ILogger<HelloController> logger)
        {
            Logger = logger;

            Logger.LogInformation("Hello World In Values Controller");

            string contextToRun = config["ContextToRun"];

            if (contextToRun.Trim() == "Console")
                context = new Context(new ConsoleRepository());
            else if (contextToRun.Trim() == "EF")
                context = new Context(new EFRepository());
            else
                context = new Context(new DBRepository());
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            context.SayHelloInterface();
            return Ok(context.SayHelloInterface());
        }

        // POST api/hello
        [HttpPost]
        public int Post([FromBody] Person person)
        {
            return context.UpdatePersonInterface(person);
        }

        // PUT api/hello/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/hello/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
