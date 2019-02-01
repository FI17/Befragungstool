using Domain;

namespace Umfrage_Tool
{
    public class AnsweringToModelTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();

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