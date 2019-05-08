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
    public class ChapterToModelTransformer
    {
        QuestionToModelChoiceTransformer questionModelTransformer = new QuestionToModelChoiceTransformer();
        

        public ICollection<ChapterViewModel> ListTransform(ICollection<Chapter> inputs)
        {
            return inputs?.Select(Transform).ToList();

        }

        public ChapterViewModel Transform(Chapter chapter)
        {
            var model = new ChapterViewModel();
            model.questionViewModels = new List<QuestionViewModel>();
            model = Transformer(chapter, model);
            return model;
        }

        ChapterViewModel Transformer(Chapter chapter, ChapterViewModel model)
        {
            model.ID = chapter.ID;
            model.text = chapter.text;
            model.position = chapter.position;
            model.questionViewModels = questionModelTransformer.ListTransform(chapter.questions);

            return model;
        }
    }
}