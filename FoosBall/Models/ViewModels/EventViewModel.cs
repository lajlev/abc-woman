namespace FoosBall.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Base;
    using Domain;

    public class EventViewModel : BaseViewModel
    {
        private readonly List<string> sentences = new List<string>
                                             {
                                                 "was how",
                                                 "shows how",
                                                 "states that",
                                                 "testifies that",
                                                 "is a sign that",
                                                 "means that"
                                             };

        private readonly List<string> beatings = new List<string>
                                             {
                                                 "beat up",
                                                 "took down",
                                                 "humiliated",
                                                 "bamboozled",
                                                 "hoodwinked",
                                                 "terminated",
                                                 "eradicated",
                                                 "owned",
                                                 "whipped",
                                                 "monster killed",
                                                 "ripped off"
                                             };

        public EventViewModel()
        {
            RandomSentence = GetSentence();
            RandomBeating = GetBeating();
        }

        public string RandomSentence { get; set; }

        public string RandomBeating { get; set; }

        public Event Event { get; set; }

        private static readonly Random RandomInstance = new Random();

        public string GetSentence(int index = -1)
        {
            var sentence = index < 0 ? sentences[RandomInstance.Next(0, sentences.Count)] : sentences[index];

            return sentence;
        }

        public string GetBeating(int index = -1)
        {
            var beating = index < 0 ? beatings[RandomInstance.Next(0, beatings.Count)] : beatings[index];

            return beating;
        }
    }
}
