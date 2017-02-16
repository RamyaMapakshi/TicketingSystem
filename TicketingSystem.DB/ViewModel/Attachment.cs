namespace TicketingSystem.DB.ViewModel
{
    public class Attachment
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public User UploadedBy { get; set; }
        public int TicketId { get; set; }
    }
}
