using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Umfrage_Tool
{
    public class AnswerViewModel
    {
        public Guid ID { get; set; }
        public int position { get; set; }
        public QuestionViewModel question { get; set; }
        public string Text { get; set; }
    }
}