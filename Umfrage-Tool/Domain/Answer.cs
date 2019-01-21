using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Answer : Entity
    {
        public string Text { get; set; }
        public int position { get; set; }
        public Question question { get; set; }
    }
}
