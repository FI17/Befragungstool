using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class QuestionToModelTransformer  
    {
        AnswerToModelTransformer modelTransformer = new AnswerToModelTransformer();
        AnsweringToModelCleanTransformer modelAnsweringTransformer = new AnsweringToModelCleanTransformer(); 

        public ICollection<QuestionViewModel> ListTransform(ICollection<Question> inputs)
        {
            if (inputs != null)
            {
                ICollection<QuestionViewModel> output = new List<QuestionViewModel>();
                foreach (Question input in inputs)
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

        public QuestionViewModel Transform(Question question)
        {
            var model = new QuestionViewModel();
            model = Transformer(model, question);
            return model;
        }

        private QuestionViewModel Transformer(QuestionViewModel model, Question question)
        {
            model.ID = question.ID;
            model.text = question.text;
            model.type = question.type;
            model.position = question.position;
            model.choices = modelTransformer.ListTransform(question.choice);
            model.givenAnswerViewModels = modelAnsweringTransformer.ListTransform(question.givenAnswer);
            model.scaleLength = question.scaleLength;
            return model;
        }
    }
}