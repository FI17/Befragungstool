using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class AnswerToModelTransformer
    {
        public ICollection<AnswerViewModel> ListTransform(ICollection<Answer> inputs)
        {
            if (inputs != null)
            {
                ICollection<AnswerViewModel> output = new List<AnswerViewModel>();
                foreach (Answer input in inputs)
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

        public AnswerViewModel Transform(Answer answer)
        {
            var model = new AnswerViewModel();
            model = Transformer(model, answer);
            return model;
        }

        private AnswerViewModel Transformer(AnswerViewModel model, Answer answer)
        {
            model.ID = answer.ID;
            model.position = answer.position;
            model.text = answer.text;
            return model;
        }
    }
}