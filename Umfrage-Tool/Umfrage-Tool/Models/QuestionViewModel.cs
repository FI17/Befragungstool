﻿using System;
using System.Collections.Generic;
using Domain;

namespace Umfrage_Tool
{
    public class QuestionViewModel
    {
        public Guid ID { get; set; }
        public string text { get; set; }
        public Question.choices typ { get; set; }
        public int position { get; set; }
        public SurveyViewModel surveyViewModel { get; set; }

        public ICollection<AnswerViewModel> answers { get; set; }
        public ICollection<AnsweringViewModel> answeringViewModels { get; set; }
    }
}