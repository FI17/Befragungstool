using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SurveyToModelSessionTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();

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
            if (survey.questions != null)
            {
                foreach (var question in survey.questions)
                {
                    model.questionViewModels.Add(modelTransformer.Transform(question));
                }
            }
            return model;
        }
    }
}