using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoosBall.Models
{
    public class HomeModels
    {

    }

    public static class WelcomeMessages
    {
        private static readonly List<string> Messages = new List<string>()
                                             {
                                                 "Welcome to the FoosBall Tournament App!!",
                                                 "Are you ready to score The Goal Of The Year?",
                                                 "Welcome FoosBall Fighter, We've been expecting you.",
                                                 "Ready to challenge another FoosBall Fighter?",
                                                 "Come in, set up a match, enjoy!",
                                             };

        public static string GetMessage(int index = -1)
        {
            if (index < 0)
            {
                return Messages[new Random().Next(0, Messages.Count)];
            }
            
            return Messages[index];
        }
    }
}