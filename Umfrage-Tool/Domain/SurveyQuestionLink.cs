using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Domain
{
    public class SurveyQuestionLink : Entity
    {
        public int position { get; set; }
        public ICollection<Answering> Answerings { get; set; }
        public Survey survey { get; set; }
        public Question question { get; set; }
    }
}
