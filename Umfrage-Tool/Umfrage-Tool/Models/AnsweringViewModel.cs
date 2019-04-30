using System;

namespace Umfrage_Tool
{
    public class AnsweringViewModel
    {
        public Guid ID { get; set; }
        public string text { get; set; }
        public SessionViewModel sessionViewModel { get; set; }
        public QuestionViewModel questionViewModel { get; set; }

    }
}