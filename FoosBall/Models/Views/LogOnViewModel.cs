namespace FoosBall.Models.Views
{
    using System;

    public class LogOnViewModel
    {
        public LogOnViewModel()
        {
            this.LogOnError = false;
            this.RememberMe = true;
        }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string RefUrl { get; set; }
        
        public bool RememberMe { get; set; }
        
        public bool LogOnError { get; set; }
    }
}