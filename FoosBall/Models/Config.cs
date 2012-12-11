namespace FoosBall.Models
{
    using FoosBall.Models.Base;

    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class Config : FoosBallDoc
    {
        public Config()
        {
            this.Name = "FoosBall Fighting";
            this.Domain = "trustpilot.com";
            this.AdminAccount = "jbe@trustpilot.com";
            this.EnableDomainValidation = true;
            this.EnableOneOnOneMatches = true;
            this.EnableGenderSpecificMatches = false;
        }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string AdminAccount { get; set; } 

        public bool EnableDomainValidation { get; set; }

        public bool EnableOneOnOneMatches { get; set; }

        public bool EnableGenderSpecificMatches { get; set; }

        public Environment Environment { get; set; }
    }
}