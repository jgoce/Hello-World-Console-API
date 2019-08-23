using HelloWorldConsole.Model;
using System;
using System.Collections.Generic;

namespace HelloWorldConsole.BLL
{
    public abstract class IRepository
    {
        public abstract string SayHello();

        public abstract int UpdatePerson(Person person);
    }
}
