using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class AnswerToModelTransformer
    {
        public ICollection<ChoiceViewModel> ListTransform(ICollection<Choice> inputs)
        {
            return inputs?.Select(Transform).ToList();
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