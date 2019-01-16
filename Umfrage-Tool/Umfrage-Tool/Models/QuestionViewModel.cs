using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace Umfrage_Tool
{
    public class QuestionViewModel
    {
        public Guid ID { get; set; }
        public string Text { get; set; }
        public choices Typ { get; set; }
        public ICollection<AnswerViewModel> Answers { get; set; }
        public ICollection<SurveyQuestionLinkViewModel> SurveyQuestionLinks { get; set; }
    }
}