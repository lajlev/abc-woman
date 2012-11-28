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
            this.RequireDomainValidation = true;
            this.AllowOneOnOneMatches = true;
            this.GenderSpecificMatches = false;
        }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string AdminAccount { get; set; } 

        public bool RequireDomainValidation { get; set; }

        public bool AllowOneOnOneMatches { get; set; }

        public bool GenderSpecificMatches { get; set; }

        public Environment Environment { get; set; }
    }
}