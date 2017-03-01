namespace TicketingSystem.DB.ViewModel
{
    public class Category
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }
}

