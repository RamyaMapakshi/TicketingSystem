namespace TicketingSystem.DB.ViewModel
{
    public class EmailParseRule
    {
        public int ID { get; set; }
        public string ParsingRule { get; set; }
        public ParseRuleType ParseType { get; set; }
        public ParseRuleProperty ParseProperty { get; set; }
        public ParseRuleProperty ExceptionParseProperty { get; set; }
        public ParseRuleType ExceptionType { get; set; }
        public string ExceptionToRule { get; set; }
        public bool? IsActive { get; set; }
    }
    public enum ParseRuleProperty
    {
        Default, To, Subject, Body, CC, Common, From, Attachment
    }
    public enum ParseRuleType
    {
        Default, Contains, DoesNotContains, Equals, DoesNotEquals, StartsWith, EndsWith, IsEmpty, CloseTicket, NeedsTicketStatus
    }
}
