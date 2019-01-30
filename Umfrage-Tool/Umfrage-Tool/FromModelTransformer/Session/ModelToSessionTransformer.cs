using Domain;

namespace Umfrage_Tool
{
    public class ModelToSessionTransformer
    {
        ModelToSurveyTransformer surveyTransformer = new ModelToSurveyTransformer();
        ModelToAnsweringTransformer answeringTransformer = new ModelToAnsweringTransformer();

        public Session Transform(SessionViewModel model)
        {
            Session session = new Session();
            session = Transformer(session, model);
            return session;
        }

        private Session Transformer(Session session, SessionViewModel model)
        {
            session.survey = surveyTransformer.Transform(model.surveyviewModel);
            if (model.answeringViewModels != null)
            {
                foreach (var answering in model.answeringViewModels)
                {
                    session.answerings.Add(answeringTransformer.Transform(answering));
                }
            }

            return session;
        }
    }
}