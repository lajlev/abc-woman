namespace FoosBall.Models.Base
{
    using Domain;

    public class SessionInfo
    {
        public User User { get; set; }

        public bool IsLoggedIn { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}