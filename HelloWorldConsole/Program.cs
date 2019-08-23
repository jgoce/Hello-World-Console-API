using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace HelloWorldConsole
{
    class Program
    {
        static void Main()
        {
            RunProgram rProgram = new RunProgram();
            rProgram.Run();
        }

    }

}
