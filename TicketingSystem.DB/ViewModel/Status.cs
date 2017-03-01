namespace TicketingSystem.DB.ViewModel
{
    public  class Status
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }

    }
}
