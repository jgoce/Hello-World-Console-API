using HelloWorldConsole.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldConsole.BLL
{
    public class Context
    {
        private IRepository _repository;

        // Constructor

        public Context(IRepository strategy)
        {
            this._repository = strategy;
        }

        public string SayHelloInterface()
        {
           return _repository.SayHello();
        }

        public int UpdatePersonInterface(Person person)
        {
            return _repository.UpdatePerson(person);
        }
    }
}
