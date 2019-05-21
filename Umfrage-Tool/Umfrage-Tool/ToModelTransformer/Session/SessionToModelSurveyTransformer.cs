using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class SessionToModelSurveyTransformer
    {
        

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

            return model;
        }
    }
}