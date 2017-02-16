namespace TicketingSystem.DB.ViewModel
{
    public class EmailRule
    {
        public int ID { get; set; }

        public string ParsingRule { get; set; }

        public RuleType ParseType { get; set; }

        public RuleType ExceptionType { get; set; }

        public string ExceptionToRule { get; set; }

        public bool? IsActive { get; set; }
    }
    public enum RuleType
    {
        Sender, Subject, Body, CC, Common
    }
}
