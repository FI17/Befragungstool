using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class ModelToSessionTransformer
    {
        ModelToSurveyTransformer surveyTransformer = new ModelToSurveyTransformer();
        ModelToAnsweringTransformer answeringTransformer = new ModelToAnsweringTransformer();

        public ICollection<Session> ListTransform(ICollection<SessionViewModel> inputs)
        {
            return inputs?.Select(Transform).ToList();
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
            session.givenAnswer = answeringTransformer.ListTransform(model.givenAnswerViewModels);

            return session;
        }
    }
}