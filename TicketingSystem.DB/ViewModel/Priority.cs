namespace TicketingSystem.DB.ViewModel
{
    public class Priority
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? DaysDue { get; set; }

        public string Color { get; set; }
        public bool? IsDefault { get; set; }

        public bool? IsActive { get; set; }

    }
}
