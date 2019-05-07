using System.Collections.Generic;

namespace Domain
{
    public class Question : Entity
    {
        public string text { get; set; }
        public choices type { get; set; }
        public Survey survey { get; set; }
        public int position { get; set; }
        public int chapter { get; set; }

        public ICollection<Choice> choice { get; set; }
        public ICollection<GivenAnswer> givenAnswer { get; set; }
        public int scaleLength { get; set; }

        public enum choices
        {
            Freitext = 0,
            MultipleOne = 1,
            Skalenfrage = 2,
            MultipleMore = 3,
            MultipleOneMitSonstiges = 4,
            MultipleMoreMitSonstiges = 5
        }
    }
}
