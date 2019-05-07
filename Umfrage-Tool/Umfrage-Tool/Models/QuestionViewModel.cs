using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace Umfrage_Tool
{
    public class QuestionViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "Fragetext")]
        public string text { get; set; }
        [Display(Name = "Fragetyp")]
        public Question.choices type { get; set; }
        public int position { get; set; }
        public int chapter { get; set; }
        public SurveyViewModel surveyViewModel { get; set; }

        public ICollection<ChoiceViewModel> choices { get; set; }
        public ICollection<GivenAnswerViewModel> givenAnswerViewModels { get; set; }
        public int scaleLength { get; set; }
    }
}