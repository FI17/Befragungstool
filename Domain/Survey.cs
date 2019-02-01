using System;
using System.Collections.Generic;

namespace Domain
{
    public class Survey : Entity
    {
        public string name { get; set; }
        public DateTime creationTime { get; protected set; } = DateTime.Now;

        public ICollection<Question> questions  { get; set; }
        public ICollection<Session> sessions { get; set; }
    }
}
