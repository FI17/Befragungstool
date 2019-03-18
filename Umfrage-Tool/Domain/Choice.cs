namespace Domain
{
    public class Choice : Entity
    {
        public string text { get; set; }
        public int position { get; set; }
        public Question question { get; set; }
        
    }
}
