using System;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SessionViewModel
    {
        public Guid ID { get; set; }
        public DateTime creationDate { get; set; }
        public SurveyViewModel surveyviewModel;

        public ICollection<AnsweringViewModel> answeringViewModels { get; set; }
    }
}