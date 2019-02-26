using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class ModelToAnsweringTransformer
    {
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();

        public ICollection<Answering> ListTransform(ICollection<AnsweringViewModel> inputs)
        {
            if (inputs != null)
            {
                ICollection<Answering> output = new List<Answering>();
                foreach (AnsweringViewModel input in inputs)
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

        public Answering Transform(AnsweringViewModel model)
        {
            Answering answering = new Answering();
            answering = Transformer(model, answering);
            return answering;
        }

        private Answering Transformer(AnsweringViewModel model, Answering answering)
        {
            answering.text = model.text;

            answering.question = questiontransformer.Transform(model.questionViewModel);

            return answering;
        }
    }
}