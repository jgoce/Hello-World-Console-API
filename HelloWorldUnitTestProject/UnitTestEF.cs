using HelloWorldConsole.BLL;
using HelloWorldConsole.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hello_WorldUnitTestProject
{
    [TestClass]
    public class UnitTestEF
    {
        Context _context = null;

        public UnitTestEF()
        {
            _context = new Context(new EFRepository());
        }

        [TestMethod]
        public void ShouldReturnHelloWorldEF()
        {            
            Assert.AreEqual("Hello from EFRepository", _context.SayHelloInterface());
        }

        [TestMethod]
        public void ShouldNotReturnHelloWorldEF()
        {
            Assert.AreNotEqual("Hello World", _context.SayHelloInterface());
        }

        [TestMethod]
        public void UpdatePersonHelloWorldEF()
        {
            Person person = new Person();
            person.FirstName = "Goce";
            person.LastName = "Jankovski";
            person.Message = "Hello from ConsoleRepository";

            Assert.AreNotEqual(-1, _context.UpdatePersonInterface(person));
        }
    }
}
