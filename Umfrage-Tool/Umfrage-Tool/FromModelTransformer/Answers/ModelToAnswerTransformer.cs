using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace Umfrage_Tool
{
    public class ModelToAnswerTransformer
    {
        public ICollection<Choice> ListTransform(ICollection<ChoiceViewModel> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public Choice Transform(ChoiceViewModel model)
        {
            var answer = new Choice();
            answer = Transformer(answer, model);
            return answer;
        }

        private Choice Transformer(Choice answer, ChoiceViewModel model)
        {
            answer.position = model.position;
            answer.text = model.text;            
            return answer;
        }
    }
}