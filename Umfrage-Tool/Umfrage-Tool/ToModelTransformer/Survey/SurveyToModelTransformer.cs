using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Acces;
using System.Data.Entity;
using Domain;

namespace Umfrage_Tool
{
    public class SurveyToModelTransformer
    {
        QuestionToModelTransformer questionModelTransformer = new QuestionToModelTransformer();
        SessionToModelTransformer sessionModelTransformer = new SessionToModelTransformer();

        public ICollection<SurveyViewModel> ListTransform(ICollection<Survey> inputs)
        {
            if (inputs != null)
            {
                ICollection<SurveyViewModel> output = new List<SurveyViewModel>();
                foreach (Survey input in inputs)
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

        public SurveyViewModel Transform(Survey survey)
        {
            var model = new SurveyViewModel();
            model.questionViewModels = new List<QuestionViewModel>();
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
            model.questionViewModels = questionModelTransformer.ListTransform(survey.questions);
            model.sessionViewModel = sessionModelTransformer.ListTransform(survey.sessions);

            return model;
        }
    }
}