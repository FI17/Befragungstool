using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class AnswerToModelTransformer
    {
        public ICollection<ChoiceViewModel> ListTransform(ICollection<Choice> inputs)
        {
            if (inputs != null)
            {
                ICollection<ChoiceViewModel> output = new List<ChoiceViewModel>();
                foreach (Choice input in inputs)
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

        public ChoiceViewModel Transform(Choice answer)
        {
            var model = new ChoiceViewModel();
            model = Transformer(model, answer);
            return model;
        }

        private ChoiceViewModel Transformer(ChoiceViewModel model, Choice answer)
        {
            model.ID = answer.ID;
            model.position = answer.position;
            model.text = answer.text;            
            return model;
        }
    }
}