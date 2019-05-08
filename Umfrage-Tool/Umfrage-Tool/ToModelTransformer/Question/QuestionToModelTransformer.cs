using Domain;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;

namespace Umfrage_Tool
{
    public class QuestionToModelTransformer  
    {
        AnswerToModelTransformer modelTransformer = new AnswerToModelTransformer();
        AnsweringToModelSessionTransformer modelAnsweringTransformer = new AnsweringToModelSessionTransformer();
        ChapterToModelTransformer chapterModelTransformer = new ChapterToModelTransformer();

        public ICollection<QuestionViewModel> ListTransform(ICollection<Question> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public QuestionViewModel Transform(Question question)
        {
            var model = new QuestionViewModel();
            model = Transformer(model, question);
            return model;
        }

        private QuestionViewModel Transformer(QuestionViewModel model, Question question)
        {
            model.ID = question.ID;
            model.text = question.text;
            model.type = question.type;
            model.position = question.position;
            if (question.chapter != null)
            {
                model.chapterViewModel = chapterModelTransformer.Transform(question.chapter);
            }
            model.choices = modelTransformer.ListTransform(question.choice);
            model.givenAnswerViewModels = modelAnsweringTransformer.ListTransform(question.givenAnswer);
            model.scaleLength = question.scaleLength;
            return model;
        }
    }
}