using System;

namespace Umfrage_Tool
{
    public class GivenAnswerViewModel
    {
        public Guid ID { get; set; }
        public string text { get; set; }
        public string[] arrayText { get; set; }
        public SessionViewModel sessionViewModel { get; set; }
        public QuestionViewModel questionViewModel { get; set; }
    }
}