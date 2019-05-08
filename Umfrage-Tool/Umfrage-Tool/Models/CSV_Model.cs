using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Umfrage_Tool
{
    public class CSV_Model
    {
        public string fragetext { get; set; }
        public string antworttext { get; set; }
        public Guid sessionID { get; set; }

        public static void DateiFürCSV(string dateiPfad, string csv_content)
        {
            File.AppendAllText(dateiPfad, csv_content.ToString(), Encoding.UTF8);
        }
    }
}