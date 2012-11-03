using System;

namespace FoosBall.Models
{
    public class LogOnModel
    {
        public LogOnModel()
        {
            LogOnError = false;
            RememberMe = true;
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RefUrl { get; set; }
        public bool RememberMe { get; set; }
        public bool LogOnError { get; set; }
    }

    public class ExistsResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public bool Exists { get; set; }
    }
}