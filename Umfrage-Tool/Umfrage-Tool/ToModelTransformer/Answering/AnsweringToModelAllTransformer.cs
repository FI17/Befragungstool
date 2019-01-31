using Domain;

namespace Umfrage_Tool
{
    public class AnsweringToModelAllTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();
        SessionToModelTransformer sessionModelTransformer = new SessionToModelTransformer();

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
            model.sessionViewModel = sessionModelTransformer.Transform(answering.session);
            return model;
        }
    }
}