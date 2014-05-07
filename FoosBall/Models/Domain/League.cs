namespace FoosBall.Models.Domain
{
    using System.Collections.Generic;
    using Base;

    public class League : FoosBallDoc
    {
        public string Name { get; set; }

        public List<string> Administrators { get; set; }
    }
}