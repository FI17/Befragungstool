using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Umfrage_Tool
{
    public class SurveyViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; protected set; }
        public ICollection<SurveyQuestionLinkViewModel> SurveyQuestionLinks { get; set; }
       
    }
}