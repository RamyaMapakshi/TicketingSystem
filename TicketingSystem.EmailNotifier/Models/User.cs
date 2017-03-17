namespace TicketingSystem.EmailNotifier.Model
{
    public class User
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string EmployeeId { get; set; }
        public bool? IsActive { get; set; }
        public string Supervisor { get; set; }
        public bool? IsExternalUser { get; set; }
        public string PhoneNumber { get; set; }

        public User()
        {
            this.IsActive = true;
        }
    }
}
