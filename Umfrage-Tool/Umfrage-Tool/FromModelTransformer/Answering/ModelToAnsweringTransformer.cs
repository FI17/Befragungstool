using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class ModelToAnsweringTransformer
    {
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();

        public ICollection<GivenAnswer> ListTransform(ICollection<GivenAnswerViewModel> inputs)
        {
            if (inputs != null)
            {
                ICollection<GivenAnswer> output = new List<GivenAnswer>();
                foreach (GivenAnswerViewModel input in inputs)
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

        public GivenAnswer Transform(GivenAnswerViewModel model)
        {
            GivenAnswer answering = new GivenAnswer();
            answering = Transformer(model, answering);
            return answering;
        }

        private GivenAnswer Transformer(GivenAnswerViewModel model, GivenAnswer answering)
        {
            answering.text = model.text;

            answering.question = questiontransformer.Transform(model.questionViewModel);

            return answering;
        }
    }
}