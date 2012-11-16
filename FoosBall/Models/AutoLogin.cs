namespace FoosBall.Models
{
    using FoosBall.Models.Base;

    public class AutoLogin : FoosBallDoc
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }
}