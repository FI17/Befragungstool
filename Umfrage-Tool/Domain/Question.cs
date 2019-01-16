using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Question : Entity
    {
        public string Text { get; set; }
        public string Typ { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<SurveyQuestionLink> SurveyQuestionLinks { get; set; }


        public Question addQuestion(Question question)
        {
            
        }
    }
}
