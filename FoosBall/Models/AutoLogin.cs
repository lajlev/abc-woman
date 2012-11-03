namespace FoosBall.Models
{
    using System;

    using FoosBall.Models.Base;

    public class AutoLogin : FoosBallDoc
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public DateTime Created { get; set; }
    }
}