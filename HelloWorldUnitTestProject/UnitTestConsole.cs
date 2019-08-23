using HelloWorldConsole.BLL;
using HelloWorldConsole.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorldConsole.Test
{
    [TestClass]
    public class UnitTestConsole
    {
        Context _context = null;

        public UnitTestConsole()
        {
            _context = new Context(new ConsoleRepository());
        }

        [TestMethod]
        public void ShouldReturnHelloWorldConsole()
        {
            Assert.AreEqual("Hello from ConsoleRepository", _context.SayHelloInterface());
        }

        [TestMethod]
        public void ShouldNotReturnHelloWorldConsole()
        {
            Assert.AreNotEqual("Hello World", _context.SayHelloInterface());
        }

        [TestMethod]
        public void UpdatePersonHelloWorldConsole()
        {
            Person person = new Person();
            person.FirstName = "Goce";
            person.LastName = "Jankovski";
            person.Message = "Hello from ConsoleRepository";

            Assert.AreEqual(-1, _context.UpdatePersonInterface(person));
        }
    }
}
