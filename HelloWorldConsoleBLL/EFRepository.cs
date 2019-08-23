using HelloWorldConsole.DAL.DataAccess;
using HelloWorldConsole.Model;
using HelloWorldConsoleDAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace HelloWorldConsole.BLL
{
    public class EFRepository : IRepository
    {
        public EFRepository()
        {            
        }

        public override string SayHello()
        {
            string sResults = "";

            try
            {
                using (var context = CreateDbContext())
                {
                    var query = context.Persons as IQueryable<Person>;
                    var personHello = query.Where(s => s.FirstName == "Hello World").FirstOrDefault<Person>();

                    sResults = personHello.Message;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return sResults;
        }
        
        public override int UpdatePerson(Person person)
        {
            try
            {
                using (var context = CreateDbContext())
                {
                    context.Persons.Add(person);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return person.PersonId;
        }

        private PersonContext CreateDbContext()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                string _connectionString = configuration.GetConnectionString("MyConnection");

                var dbBuilder = new DbContextOptionsBuilder<PersonContext>();
                dbBuilder.UseSqlServer(_connectionString);

                return new PersonContext(dbBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting DbContext connection " + ex.StackTrace);
                throw ex;
            }

        }
    }
}
