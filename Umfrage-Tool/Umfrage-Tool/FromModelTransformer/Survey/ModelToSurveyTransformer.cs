using Domain;

namespace Umfrage_Tool
{
    public class ModelToSurveyTransformer
    {
        ModelToQuestionTransformer questionTransformer = new ModelToQuestionTransformer();

        public Survey Transform(SurveyViewModel model)
        {
            Survey survey = new Survey();
            survey = Transformer(model, survey);
            return survey;
        }

        private Survey Transformer(SurveyViewModel model, Survey survey)
        {
            survey.name = model.name;
            if (model.questionViewModels != null)
            {
                foreach (var question in model.questionViewModels)
                {
                    survey.questions.Add(questionTransformer.Transform(question));
                }
            }

            return survey;
        }
    }
}