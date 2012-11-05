namespace FoosBall.Models
{
    using FoosBall.Models.Base;

    public class Config : FoosBallDoc
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Domain { get; set; }

        public bool RequireDepartment { get; set; }

        public bool RequireDomainValidation { get; set; }

        public bool AllowOneOnOneMatches { get; set; }

        public bool GenderSpecificMatches { get; set; }
    }
}