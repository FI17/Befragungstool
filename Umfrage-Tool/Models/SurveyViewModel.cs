using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Umfrage_Tool
{
    [Bind]
    public class SurveyViewModel
    {
        [ScaffoldColumn(false)]
        public Guid ID { get; set; }
        
        [DisplayName("Name des Fragebogens")]
        [Required(ErrorMessage = "Bitte geben Sie einen Namen ein")]
        [StringLength(160)]
        public string name { get; set; }
        
        
        public DateTime creationTime { get; set; }

        public ICollection<QuestionViewModel> questionViewModels { get; set; }
        public ICollection<SessionViewModel> sessionViewModel { get; set; }

    }
}