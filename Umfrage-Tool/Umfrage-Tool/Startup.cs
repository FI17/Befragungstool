using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Umfrage_Tool.Startup))]
namespace Umfrage_Tool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //Einkommentieren falls Datenbankerstellung gewünscht! weiter zu: database.cs 
            //database.CreateAndTestDatabase();
        }
    }
}

