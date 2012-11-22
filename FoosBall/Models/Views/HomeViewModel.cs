namespace FoosBall.Models.Views
{
    using System;
    using System.Collections.Generic;

    public class HomeViewModel
    {
        private static readonly List<string> Messages = new List<string>
                                             {
                                                 "Welcome to the FoosBall Tournament App!!",
                                                 "Are you ready to score The Goal Of The Year?",
                                                 "Welcome FoosBall Fighter, We've been expecting you.",
                                                 "Ready to challenge another FoosBall Fighter?",
                                                 "Come on in, set up a match, enjoy!",
                                             };
        
        public static string GetMessage(int index = -1)
        {
            return index < 0 ? Messages[new Random().Next(0, Messages.Count)] : Messages[index];
        }
    }
}