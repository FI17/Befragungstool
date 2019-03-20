﻿using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class AnsweringToModelAllTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();
        SessionToModelTransformer sessionModelTransformer = new SessionToModelTransformer();

        public ICollection<GivenAnswerViewModel> ListTransform(ICollection<GivenAnswer> inputs)
        {
            if (inputs != null)
            {

                ICollection<GivenAnswerViewModel> output = new List<GivenAnswerViewModel>();
                foreach (GivenAnswer
                    input in inputs)
                {
                    output.Add(Transform(input));
                }
                return output;
            }
            else
            {
                return null;
            }
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
            model.sessionViewModel = sessionModelTransformer.Transform(answering.session);
            
            return model;
        }
    }
}