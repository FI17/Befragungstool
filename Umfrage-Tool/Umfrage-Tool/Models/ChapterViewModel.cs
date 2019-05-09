using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;

namespace Umfrage_Tool
{
    public class ChapterViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "Text Des Kapitels")]
        public string text { get; set; }

        public SessionViewModel sessionViewModel { get; set; }
        public int position { get; set; }
      

        public ICollection<QuestionViewModel> questionViewModels { get; set; }
        
        
    }
}