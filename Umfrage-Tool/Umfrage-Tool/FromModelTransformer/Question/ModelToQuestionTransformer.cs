using Domain;

namespace Umfrage_Tool
{
    public class ModelToQuestionTransformer
    {
        ModelToAnswerTransformer answerTransformer = new ModelToAnswerTransformer();

        public Question Transform(QuestionViewModel model)
        {
            Question question = new Question();
            question = Transformer(model, question);
            return question;
        }

        private Question Transformer(QuestionViewModel model, Question question)
        {
            question.text = model.text;
            question.typ = model.typ;
            question.position = model.position;
            if (model.answers != null)
            {
                foreach (var answer in model.answers)
                {
                    question.answers.Add(answerTransformer.Transform(answer));
                }
            }
            return question;
        }
    }
}