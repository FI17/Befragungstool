using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SurveyToModelQuestionTransformer
    {
        SessionToModelTransformer modelTransformer = new SessionToModelTransformer();

        public SurveyViewModel Transform(Survey survey)
        {
            var model = new SurveyViewModel();
            model.sessionViewModel = new List<SessionViewModel>();
            model = Transformer(survey, model);
            return model;
        }

        SurveyViewModel Transformer(Survey survey, SurveyViewModel model)
        {
            model.ID = survey.ID;
            model.name = survey.name;
            model.creationTime = survey.creationTime;
            if (survey.sessions != null)
            {
                foreach (var session in survey.sessions)
                {
                    model.sessionViewModel.Add(modelTransformer.Transform(session));
                }
            }
           
            return model;
        }
    }
}