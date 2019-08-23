using HelloWorldConsole.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldConsole.BLL
{
    public class ConsoleRepository : IRepository
    {

        public override string SayHello()
        {
            return "Hello from ConsoleRepository";
        }

        public override int UpdatePerson(Person person)
        {
            return -1;
        }
    }
}
