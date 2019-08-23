using HelloWorldConsole.BLL;
using HelloWorldConsole.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorldConsole.Test
{
    [TestClass]
    public class UnitTestDB
    {
        Context _context = null;

        public UnitTestDB()
        {
            _context = new Context(new DBRepository());
        }

        [TestMethod]
        public void ShouldReturnHelloWorldDB()
        {
            Assert.AreEqual("Hello from DBRepository", _context.SayHelloInterface());
        }

        [TestMethod]
        public void ShouldNotReturnHelloWorldDB()
        {
            Assert.AreNotEqual("Hello World", _context.SayHelloInterface());
        }

        [TestMethod]
        public void UpdatePersonHelloWorldDB()
        {
            Person person = new Person();
            person.FirstName = "Goce";
            person.LastName = "Jankovski";
            person.Message = "Hello from ConsoleRepository";

            Assert.AreNotEqual(-1, _context.UpdatePersonInterface(person));
        }
    }
}
