using Domain;

namespace Umfrage_Tool
{
    public class ModelToAnsweringTransformer
    {
        ModelToQuestionTransformer questiontransformer = new ModelToQuestionTransformer();

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