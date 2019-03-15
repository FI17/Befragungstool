namespace Domain
{
    public class GivenAnswer : Entity
    {
        public string text { get; set; }
        public string[] arrayText { get; set; }
        public Session session { get; set; }
        public Question question { get; set; }
    }
}


