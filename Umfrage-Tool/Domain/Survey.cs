using System;
using System.Collections.Generic;

namespace Domain
{
    public class Survey : Entity
    {
        public string name { get; set; }
        public DateTime creationTime { get; protected set; } = DateTime.Now;
        public States states { get; set; }
        public Guid Creator { get; set; }


        public ICollection<Question> questions  { get; set; }
        public ICollection<Session> sessions { get; set; }
        public ICollection<Chapter> chapters { get; set; }

        public enum States
        {
            InBearbeitung,
            Öffentlich,
            Beendet
        }
    }
    
}
