using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class ModelToAnsweringTransformer
    {
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();

        public ICollection<GivenAnswer> ListTransform(ICollection<GivenAnswerViewModel> inputs)
        {
            return inputs?.Select(Transform).ToList();
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