using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Umfrage_Tool.Models
{
    public class FragebogenModel
    {
        public string Name { get; set; }
        public ICollection<FragenModel> FragenListe { get; set; }
    }
}