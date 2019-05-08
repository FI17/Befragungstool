using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Umfrage_Tool
{
    public class ModelToChapterTransformer
    {
        ModelToQuestionTransformer questionTransformer = new ModelToQuestionTransformer();

        public ICollection<Chapter> ListTransform(ICollection<ChapterViewModel> inputs)
        {
            return inputs?.Select(Transform).ToList();
        }

        public Chapter Transform(ChapterViewModel model)
        {
            var chapter = new Chapter();
            chapter = Transformer(chapter, model);
            return chapter;
        }

        private Chapter Transformer(Chapter chapter, ChapterViewModel model)
        {
            chapter.text = model.text;
            chapter.position = model.position;
            chapter.questions = questionTransformer.ListTransform(model.questionViewModels);

            return chapter;
        }
    }
}