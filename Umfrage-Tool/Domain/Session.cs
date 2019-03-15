using System;
using System.Collections.Generic;

namespace Domain
{
    public class Session : Entity
    {
        public Survey survey { get; set; }
        public DateTime creationTime { get; protected set; } = DateTime.Now;

        public ICollection<GivenAnswer> givenAnswer { get; set; }
    }
}
