using Domain;
using System.Collections.Generic;

namespace Umfrage_Tool
{
    public class SessionToModelTransformer
    {
        AnsweringToModelTransformer modelTransformer = new AnsweringToModelTransformer();

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
            if (session.answerings != null)
            {
                foreach (var answering in session.answerings)
                {
                    model.answeringViewModels.Add(modelTransformer.Transform(answering));
                }
            }
            return model;
        }
    }
}