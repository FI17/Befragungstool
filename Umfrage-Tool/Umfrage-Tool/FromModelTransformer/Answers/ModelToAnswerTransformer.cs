using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace Umfrage_Tool
{
    public class ModelToAnswerTransformer
    {
        public Answer Transform(AnswerViewModel model)
        {
            var answer = new Answer();
            answer = Transformer(answer, model);
            return answer;
        }

        private Answer Transformer(Answer answer, AnswerViewModel model)
        {
            answer.position = model.position;
            answer.text = model.text;
            return answer;
        }
    }
}