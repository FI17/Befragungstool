using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Answering : Entity
    {
        public string text { get; set; }
        public SurveyQuestionLink surveyQuestionLink { get; set; }
    }
}


