using HelloWorldConsole.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldConsoleDAL.EF
{
    //public class PersonContext { }
    public class PersonContext : DbContext
    {

        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        { }

        public PersonContext() { }
        public DbSet<Person> Persons { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=PersonDB;Trusted_Connection=True;");
        //}
    }
}
