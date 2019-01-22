using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Umfrage_Tool.Models
{
    public class FragenModel
    {
        public int ID { get; set; }
        public string Text { get; set;}
        public string Typ { get; set; }
        public ICollection<string> AntwortenListe { get; set;}
}
}