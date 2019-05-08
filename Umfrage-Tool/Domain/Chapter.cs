using System;
using System.Collections.Generic;

namespace Domain
{
    public class Chapter : Entity
    {
        public string text { get; set; }
        public int position { get; set; }
        public Survey survey { get; set; }

        public ICollection<Question> questions  { get; set; }
    }
    
}
