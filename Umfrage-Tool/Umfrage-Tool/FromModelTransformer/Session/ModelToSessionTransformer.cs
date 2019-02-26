using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class ModelToSessionTransformer
    {
        ModelToSurveyTransformer surveyTransformer = new ModelToSurveyTransformer();
        ModelToAnsweringTransformer answeringTransformer = new ModelToAnsweringTransformer();

        public ICollection<Session> ListTransform(ICollection<SessionViewModel> inputs)
        {
            if (inputs != null)
            {
                ICollection<Session> output = new List<Session>();
                foreach (SessionViewModel input in inputs)
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

        public Session Transform(SessionViewModel model)
        {
            Session session = new Session();
            session = Transformer(session, model);
            return session;
        }

        private Session Transformer(Session session, SessionViewModel model)
        {
            session.survey = surveyTransformer.Transform(model.surveyviewModel);
            session.answerings = answeringTransformer.ListTransform(model.answeringViewModels);

            return session;
        }
    }
}