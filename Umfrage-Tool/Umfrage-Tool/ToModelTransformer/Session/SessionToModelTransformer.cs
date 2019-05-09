using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class SessionToModelTransformer
    {
        AnsweringToModelTransformer modelTransformer = new AnsweringToModelTransformer();
        SurveyToModelSessionTransformer surveyTransformer = new SurveyToModelSessionTransformer();

        public ICollection<SessionViewModel> ListTransform(ICollection<Session> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public SessionViewModel Transform(Session session)
        {
            var model = new SessionViewModel();
            model.givenAnswerViewModels = new List<GivenAnswerViewModel>();
            model = Tranformer(session, model);
            return model;
        }

        private SessionViewModel Tranformer(Session session, SessionViewModel model)
        {
            model.ID = session.ID;
            model.creationDate = session.creationTime;
            model.givenAnswerViewModels = modelTransformer.ListTransform(session.givenAnswer);

            return model;
        }
    }
}