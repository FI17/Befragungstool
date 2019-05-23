using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class SurveyToModelQuestionTransformer
    {
        SessionToModelTransformer modelTransformer = new SessionToModelTransformer();

        public ICollection<SurveyViewModel> ListTransform(ICollection<Survey> inputs)
        {
            return inputs?.Select(Transform).ToList();

        }

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
            model.Creator = survey.Creator;
            model.states = survey.states;
            model.endTime = survey.endTime;
            model.releaseTime = survey.releaseTime;
            model.sessionViewModel = modelTransformer.ListTransform(survey.sessions);

            return model;
        }
    }
}