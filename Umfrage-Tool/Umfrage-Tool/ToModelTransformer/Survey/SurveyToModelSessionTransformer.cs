using Domain;

namespace Umfrage_Tool
{
    public class SurveyToModelSessionTransformer
    {
        QuestionToModelTransformer modelTransformer = new QuestionToModelTransformer();

        public SurveyViewModel Transform(Survey survey)
        {
            var model = new SurveyViewModel();
            model = Transformer(survey, model);
            return model;
        }

        SurveyViewModel Transformer(Survey survey, SurveyViewModel model)
        {
            model.ID = survey.ID;
            model.name = survey.name;
            model.creationTime = survey.creationTime;
            foreach(var question in survey.questions)
            {
                model.questionViewModels.Add(modelTransformer.Transform(question));
            }
            return model;
        }
    }
}