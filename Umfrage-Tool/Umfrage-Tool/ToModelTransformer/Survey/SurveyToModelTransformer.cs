using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

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
                    QuestionViewModel b = questionModelTransformer.Transform(question);
                    model.questionViewModels.Add(b);
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