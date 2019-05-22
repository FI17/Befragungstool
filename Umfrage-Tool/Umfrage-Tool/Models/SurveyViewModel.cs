using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace Umfrage_Tool
{
    public class SurveyViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "Name des Fragebogens")]
        public string name { get; set; }
        public DateTime creationTime { get; set; }
        public DateTime releaseTime { get; set; }
        public DateTime endTime { get; set; }

        public ICollection<QuestionViewModel> questionViewModels { get; set; }
        public ICollection<SessionViewModel> sessionViewModel { get; set; }
        public ICollection<ChapterViewModel> chapterViewModels { get; set; }
        public  Survey.States states { get; set; }
        public Guid Creator { get; set; }
        public string CreatorName { get; set; }
    }
}