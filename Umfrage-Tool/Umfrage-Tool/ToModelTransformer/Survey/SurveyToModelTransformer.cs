using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SurveyToModelTransformer
    {
        QuestionToModelTransformer questionModelTransformer = new QuestionToModelTransformer();
        SessionToModelTransformer sessionModelTransformer = new SessionToModelTransformer();

        public SurveyViewModel Transform(Survey survey)
        {
            var model = new SurveyViewModel();
            model.questionViewModels = new List<QuestionViewModel>();
            model.sessionViewModel = new List<SessionViewModel>();
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
                    model.questionViewModels.Add(questionModelTransformer.Transform(question));
                }
            }
            if (survey.sessions != null)
            {
                foreach (var session in survey.sessions)
                {
                    model.sessionViewModel.Add(sessionModelTransformer.Transform(session));
                }
            }
            return model;
        }
    }
}