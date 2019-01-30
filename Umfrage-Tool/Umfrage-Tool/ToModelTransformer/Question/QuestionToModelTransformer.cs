using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class QuestionToModelTransformer  
    {
        AnswerToModelTransformer modelTransformer = new AnswerToModelTransformer();

        public QuestionViewModel Transform(Question question)
        {
            var model = new QuestionViewModel();
            model = Transformer(model, question);
            model.answers = new List<AnswerViewModel>();
            return model;
        }

        private QuestionViewModel Transformer(QuestionViewModel model, Question question)
        {
            model.ID = question.ID;
            model.text = question.text;
            model.typ = question.typ;
            model.position = question.position;
            if (question.answers != null)
            {
                foreach (var answer in question.answers)
                {
                    model.answers.Add(modelTransformer.Transform(answer));
                }
            }

            return model;
        }
    }
}