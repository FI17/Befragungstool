using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class ModelToQuestionTransformer
    {
        ModelToAnswerTransformer answerTransformer = new ModelToAnswerTransformer();

        public ICollection<Question> ListTransform(ICollection<QuestionViewModel> inputs)
        {
            if (inputs != null)
            {
                ICollection<Question> output = new List<Question>();
                foreach (QuestionViewModel input in inputs)
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
            question.answers = answerTransformer.ListTransform(model.answers);
            
            return question;
        }
    }
}