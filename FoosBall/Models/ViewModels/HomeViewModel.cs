namespace FoosBall.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using FoosBall.Models.Base;

    public class HomeViewModel : BaseViewModel
    {
        private readonly List<string> messages = new List<string>
                                             {
                                                 "Welcome to the FoosBall Tournament App!!",
                                                 "Are you ready to score The Goal Of The Year?",
                                                 "Welcome FoosBall Fighter, We've been expecting you.",
                                                 "Ready to challenge another FoosBall Fighter?",
                                                 "Come on in, set up a match, enjoy!",
                                             };

        public HomeViewModel()
        {
            this.Message = this.GetMessage();
        }

        public string Message { get; set; }

        public string GetMessage(int index = -1)
        {
            return index < 0 ? messages[new Random().Next(0, messages.Count)] : messages[index];
        }
    }
}