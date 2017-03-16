namespace TicketingSystem.DB.ViewModel
{
    public class EmailRule
    {
        public int ID { get; set; }
        public string ParsingRule { get; set; }
        public RuleType ParseType { get; set; }
        public RuleProperty ParseProperty { get; set; }
        public RuleProperty ExceptionParseProperty { get; set; }
        public RuleType ExceptionType { get; set; }
        public string ExceptionToRule { get; set; }
        public bool? IsActive { get; set; }
    }
    public enum RuleProperty
    {
        To, Subject, Body, CC, Common,From,Attachment
    }
    public enum RuleType
    {
        Excludes,Includes,Contains,DoesNotContains
    }
}
