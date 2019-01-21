using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Umfrage_Tool
{
    public class AnsweringViewModel
    {
        public Guid ID { get; set; }
        public string text { get; set; }
        public SurveyQuestionLinkViewModel surveyQuestionLink { get; set; }
    }
}