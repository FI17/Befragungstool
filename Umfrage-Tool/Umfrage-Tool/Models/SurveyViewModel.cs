using System;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SurveyViewModel
    {
        public Guid ID { get; set; }
        public string name { get; set; }
        public DateTime creationTime { get; set; }

        public ICollection<QuestionViewModel> questionViewModels { get; set; }
        public ICollection<SessionViewModel> sessionViewModel { get; set; }

    }
}