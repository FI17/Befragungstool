using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Survey : Entity
    {
        public string Name { get; set; }
        public DateTime CreationTime { get; protected set; } = DateTime.Now;
        public ICollection<SurveyQuestionLink> SurveyQuestionLinks  { get; set; }
    }
}
