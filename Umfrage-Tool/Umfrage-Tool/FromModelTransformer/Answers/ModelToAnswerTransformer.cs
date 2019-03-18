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
            if (inputs != null)
            {
                ICollection<Choice> output = new List<Choice>();
                foreach (ChoiceViewModel input in inputs)
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