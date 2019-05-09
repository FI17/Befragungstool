namespace Domain
{
    public class Answering : Entity
    {
        public string text { get; set; }
        public Session session { get; set; }
        public Question question { get; set; }
    }
}


