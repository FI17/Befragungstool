using System.Collections.Generic;

namespace Domain
{
    public class Question : Entity
    {
        public string text { get; set; }
        public choices typ { get; set; }
        public Survey survey { get; set; }
        public int position { get; set; }

        public ICollection<Answer> answers { get; set; }
        public ICollection<Answering> answerings { get; set; }


        public enum choices
        {
            Freitext = 0,
            MultipleOne = 1,
            Skalenfrage = 2
        } 
        
    }
}
