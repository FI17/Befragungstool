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

            //Einkommentieren falls Datenbankerstellung gewünscht ist! weiter zu: Startup.cs
            //data.Database.CreateIfNotExists();
        }
    }
}