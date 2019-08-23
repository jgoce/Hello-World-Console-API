using HelloWorldConsole.BLL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HelloWorldConsole
{
    class RunProgram
    {
        ILogger logger = null;

        public RunProgram()
        {
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();
            logger = loggerFactory.CreateLogger<RunProgram>();
        }

        public void Run()
        {
            logger.LogInformation("Running RunProgram.");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            logger.LogInformation("ContextToRun RunProgram = " + config["ContextToRun"]);

            if (config["ContextToRun"] == "WebAPI")
                CallAPITask();
            else if (config["ContextToRun"] == "Console")
                new Context(new ConsoleRepository()).SayHelloInterface();
            else if (config["ContextToRun"] == "EF")
                new Context(new EFRepository()).SayHelloInterface();
            else
                new Context(new DBRepository()).SayHelloInterface();

            Console.ReadKey();
        }

        private void CallAPITask()
        {
            logger.LogInformation("Calling WebAPI from CallAPITask");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53886");
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("/api/hello").Result;
                string stringData = response.Content.ReadAsStringAsync().Result;

            }
        }
    }
}
