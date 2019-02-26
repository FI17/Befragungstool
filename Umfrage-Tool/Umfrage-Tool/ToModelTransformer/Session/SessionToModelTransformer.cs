using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SessionToModelTransformer
    {
        AnsweringToModelTransformer modelTransformer = new AnsweringToModelTransformer();
        SurveyToModelSessionTransformer surveyTransformer = new SurveyToModelSessionTransformer();

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
            model.answeringViewModels = new List<AnsweringViewModel>();
            model = Tranformer(session, model);
            return model;
        }

        private SessionViewModel Tranformer(Session session, SessionViewModel model)
        {
            model.ID = session.ID;
            model.creationDate = session.creationTime;
            model.answeringViewModels = modelTransformer.ListTransform(session.answerings);

            return model;
        }
    }
}