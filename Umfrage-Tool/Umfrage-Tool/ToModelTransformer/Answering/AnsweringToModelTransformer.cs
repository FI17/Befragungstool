﻿using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class AnsweringToModelTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();

        public ICollection<GivenAnswerViewModel> ListTransform(ICollection<GivenAnswer> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public GivenAnswerViewModel Transform(GivenAnswer answering)
        {
            var model = new GivenAnswerViewModel();
            model = Transform(model, answering);
            return model;
        }

        private GivenAnswerViewModel Transform(GivenAnswerViewModel model, GivenAnswer answering)
        {
            model.ID = answering.ID;
            model.text = answering.text;
            model.questionViewModel = modelTransformer.Transform(answering.question);
            return model;
        }
    }
}