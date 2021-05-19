namespace SomeShop.DAL.Models
{
    public class ChatAdministrator : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public long ChatId { get; set; }
    }
}