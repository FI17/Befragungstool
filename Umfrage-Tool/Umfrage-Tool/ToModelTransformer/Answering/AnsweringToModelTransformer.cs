using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class AnsweringToModelTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();

        public ICollection<AnsweringViewModel> ListTransform(ICollection<Answering> inputs)
        {
            if (inputs != null)
            {

                ICollection<AnsweringViewModel> output = new List<AnsweringViewModel>();
                foreach (Answering
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

        public AnsweringViewModel Transform(Answering answering)
        {
            var model = new AnsweringViewModel();
            model = Transform(model, answering);
            return model;
        }

        private AnsweringViewModel Transform(AnsweringViewModel model, Answering answering)
        {
            model.ID = answering.ID;
            model.text = answering.text;
            model.questionViewModel = modelTransformer.Transform(answering.question);
            return model;
        }
    }
}