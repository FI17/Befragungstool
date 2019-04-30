using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class ModelToSurveyTransformer
    {
        ModelToQuestionTransformer questionTransformer = new ModelToQuestionTransformer();

        public ICollection<Survey> ListTransform(ICollection<SurveyViewModel> inputs)
        {
            if (inputs != null)
            {
                ICollection<Survey> output = new List<Survey>();
                foreach (SurveyViewModel input in inputs)
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

        public Survey Transform(SurveyViewModel model)
        {
            Survey survey = new Survey();
            survey.questions = new List<Question>();
            survey = Transformer(model, survey);
            return survey;
        }

        private Survey Transformer(SurveyViewModel model, Survey survey)
        {
            survey.name = model.name;
            survey.states = model.states;
            survey.Creator = model.Creator;
            survey.questions = questionTransformer.ListTransform(model.questionViewModels);

            return survey;
        }
    }
}