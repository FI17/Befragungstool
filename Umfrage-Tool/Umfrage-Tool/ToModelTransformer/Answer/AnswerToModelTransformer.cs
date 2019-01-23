using Domain;

namespace Umfrage_Tool
{
    public class AnswerToModelTransformer
    {
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