using System;
using System.Linq;
using System.Data.Entity;
using Domain.Acces;
using Domain;
using System.Collections;

namespace Umfrage_Tool
{
    internal class database
    {
        internal static void CreateAndTestDatabase()
        {
            var data = new Domain.Acces.DatabaseContent();

            //Einkommentieren falls Datenbankerstellung gewünscht ist! weiter zu: Startup.cs
            data.Database.CreateIfNotExists();

            //var sd = new Guid("150603ac-a13d-4574-9f69-a52049403c90");
            //var s = data.Surveys.Include(h => h.sessions.Select(t => t.answerings)).FirstOrDefault(a => a.ID == sd);
            //Question d = new Question { text = "Test" };
            ////s.sessions.Add(new Session());

            //data.SaveChanges();
            //var f = data.Surveys.Include(dlkdfj => dlkdfj.questions).First();
            //f.questions.Add(d);
            //data.SaveChanges();

            //var q = data.Questions.First();
            //var p = data.Answerings.First();
            //p.question = q;
            //data.SaveChanges();








        }
    }
}