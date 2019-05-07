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
            question.type = model.type;
            question.position = model.position;
            question.chapter = model.chapter;
            question.choice = answerTransformer.ListTransform(model.choices);
            question.scaleLength = model.scaleLength;
            return question;
        }
    }
}