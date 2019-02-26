using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SurveyToModelSessionTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();

        public ICollection<SurveyViewModel> ListTransform(ICollection<Survey> inputs)
        {
            if (inputs != null)
            {
                ICollection<SurveyViewModel> output = new List<SurveyViewModel>();
                foreach (Survey input in inputs)
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

        public SurveyViewModel Transform(Survey survey)
        {
            var model = new SurveyViewModel();
            model.questionViewModels = new List<QuestionViewModel>();
            model = Transformer(survey, model);
            return model;
        }

        SurveyViewModel Transformer(Survey survey, SurveyViewModel model)
        {
            model.ID = survey.ID;
            model.name = survey.name;
            model.creationTime = survey.creationTime;
            model.questionViewModels = modelTransformer.ListTransform(survey.questions);

            return model;
        }
    }
}