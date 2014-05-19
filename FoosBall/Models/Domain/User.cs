namespace FoosBall.Models.Domain
{
    using Base;
    using Main;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class User : FoosBallDoc
    {
        public User()
        {
            Deactivated = false;
            RememberMe = true;
        }

        public User(string id)
        {
            Id = id;
            Deactivated = false;
            RememberMe = true;
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Deactivated { get; set; }

        public bool RememberMe { get; set; }

        public string GravatarUrl
        {
            get
            {
                return !string.IsNullOrEmpty(Email) ? Md5.GetGravatarUrl(Email) : string.Empty;
            }
        }
    }
}
