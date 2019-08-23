using HelloWorldConsole.DAL.DataAccess;
using HelloWorldConsole.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;

namespace HelloWorldConsole.BLL
{
    public class DBRepository : IRepository
    {

        public override string SayHello()
        {
            string sResults = "";

            try
            {
                using (DataConnect dc1 = new DataConnect())
                {
                    // get select row from DB
                    DataRow rowResults = dc1.ExecuteFunction<DataTable>("SELECT 'Hello from DBRepository' as message", null).Rows[0];
                    sResults = string.Format("Result from DB: {0}" ,(rowResults["message"].ToString()));
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
            var dataResults = new DataTable();
            dynamic sqlParam = new ExpandoObject();
            sqlParam.firstName = person.FirstName;
            sqlParam.lastName = person.LastName;

            try
            {
                using (DataConnect dc1 = new DataConnect())
                {
                    dataResults = dc1.ExecuteFunction<DataTable>("exec some_sp @firstName, @lastName", sqlParam);
                    person.PersonId = Int32.Parse(dataResults.Rows[0]["PersonID"].ToString());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return person.PersonId;
        }
    }
}
