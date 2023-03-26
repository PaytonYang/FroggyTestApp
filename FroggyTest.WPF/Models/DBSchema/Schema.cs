namespace FroggyTest.WPF.Models.DBSchema
{
    public class AuthorityTable
    {
        public long AuthorityId { get; set; }
        public string AuthorityName { get; set; } = "Guest";
    }

    public class UserTable
    {
        public long UserId { get; set;}
        public string UserName { get; set; } = "";
        public string UserPassword { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public long AuthorityId { get; set; }
    }
}
