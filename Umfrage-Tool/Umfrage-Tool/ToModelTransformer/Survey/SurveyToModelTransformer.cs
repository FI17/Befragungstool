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
        ChapterToModelTransformer chapterModelTransformer = new ChapterToModelTransformer();

        public ICollection<SurveyViewModel> ListTransform(ICollection<Survey> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public SurveyViewModel Transform(Survey survey)
        {
            var model = new SurveyViewModel
            {
                questionViewModels = new List<QuestionViewModel>(),
                sessionViewModel = new List<SessionViewModel>()
            };
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
            model.endTime = survey.endTime;
            model.releaseTime = survey.releaseTime;
            model.questionViewModels = questionModelTransformer.ListTransform(survey.questions);
            model.sessionViewModel = sessionModelTransformer.ListTransform(survey.sessions);
            model.chapterViewModels = chapterModelTransformer.ListTransform(survey.chapters);

            return model;
        }
    }
}