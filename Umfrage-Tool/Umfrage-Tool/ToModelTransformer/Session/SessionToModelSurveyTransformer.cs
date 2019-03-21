using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SessionToModelSurveyTransformer
    {
        

        public ICollection<SessionViewModel> ListTransform(ICollection<Session> inputs)
        {
            if (inputs != null)
            {

                ICollection<SessionViewModel> output = new List<SessionViewModel>();
                foreach (Session input in inputs)
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