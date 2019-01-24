using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Umfrage_Tool
{
    public class SurveyViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "Name des Fragebogens")]
        public string name { get; set; }
        public DateTime creationTime { get; set; }

        public ICollection<QuestionViewModel> questionViewModels { get; set; }
        public ICollection<SessionViewModel> sessionViewModel { get; set; }

    }
}