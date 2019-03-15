using System;

namespace Umfrage_Tool
{
    public class ChoiceViewModel
    {
        public Guid ID { get; set; }
        public string text { get; set; }
        public int position { get; set; }
        public QuestionViewModel question { get; set; }
        public string[] arrayText { get; set; }
    }
}