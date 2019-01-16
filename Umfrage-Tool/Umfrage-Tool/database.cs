using System;
using System.Linq;
using System.Data.Entity;
using Domain.Acces;
using Domain;

namespace Umfrage_Tool
{
    internal class database
    {
        internal static void CreateAndTestDatabase()
        {
            var data = new Domain.Acces.DatabaseContent();


            //data.Database.Delete();
            //data.Database.Create();

            //catch (Exception)
            // {

            //   data.Database.Create();
            // }

            var question 
            var survey = new Survey() { Name = "erste umfrage" };
            


        }
    }
}