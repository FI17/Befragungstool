using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class ModelToQuestionTransformer
    {
        ModelToAnswerTransformer answerTransformer = new ModelToAnswerTransformer();

        public ICollection<Question> ListTransform(ICollection<QuestionViewModel> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public Question Transform(QuestionViewModel model)
        {
            Question question = new Question();
            question = Transformer(model, question);
            return question;
        }

        private Question Transformer(QuestionViewModel model, Question question)
        {
            question.text = model.text;
            question.type = model.type;
            question.position = model.position;
            question.choice = answerTransformer.ListTransform(model.choices);
            question.scaleLength = model.scaleLength;
            return question;
        }
    }
}