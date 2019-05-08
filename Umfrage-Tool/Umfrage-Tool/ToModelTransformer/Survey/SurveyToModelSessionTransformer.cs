using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class SurveyToModelSessionTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();
        ChapterToModelTransformer chapterModelTransformer = new ChapterToModelTransformer();

        public ICollection<SurveyViewModel> ListTransform(ICollection<Survey> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public SurveyViewModel Transform(Survey survey)
        {
            var model = new SurveyViewModel {questionViewModels = new List<QuestionViewModel>()};
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
            model.questionViewModels = modelTransformer.ListTransform(survey.questions);
            model.chapterViewModels = chapterModelTransformer.ListTransform(survey.chapters);

            return model;
        }
    }
}