using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Umfrage_Tool
{
    public class SurveyQuestionLinkViewModel
    {
        public Guid ID { get; set; }
        public int position { get; set; }
        public ICollection<AnsweringViewModel> Answerings { get; set; }
        public SurveyViewModel survey { get; set; }
        public QuestionViewModel question { get; set; }
    }
}