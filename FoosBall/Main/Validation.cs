namespace FoosBall.Main
{
    using System.Linq;
    using Models.Base;
    using Models.Domain;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    public class Validation
    {
        private readonly MongoDatabase Dbh;

        public Validation()
        {
            var environment = AppConfig.GetEnvironment();

            this.Dbh = new Db(environment).Dbh;
            this.Settings = this.Dbh.GetCollection<Config>("Config").FindOne();
            this.Settings.Environment = environment;
        }

        private Config Settings { get; set; }
        
        public bool ValidateEmail(string email)
        {
            var domain = "@" + this.Settings.Domain;

            return !string.IsNullOrEmpty(email) &&
                   email.EndsWith(domain) &&
                   email.Count(x => x == '@') == 1 &&
                   email.Length > domain.Length &&
                   !email.Contains(" ");
        }

        public AjaxResponse ValidateNewUserData(string email, string name, string password)
        {
            var response = new AjaxResponse { Success = false };

            if (!this.ValidateEmail(email))
            {
                response.Message = "You must provide a valid trustpilot email";
                return response;
            }

            if (this.PlayerEmailExists(email))
            {
                response.Message = "This email is already registered";
                return response;
            }

            if (string.IsNullOrEmpty(name))
            {
                response.Message = "You must provide a name";
                return response;
            }

            if (this.PlayerNameExists(name))
            {
                response.Message = "A player with this name is already registered";
                return response;
            }

            if (string.IsNullOrEmpty(password))
            {
                response.Message = "You must provide a password";
                return response;
            }

            response.Message = "User created succesfully";
            response.Success = true;

            return response;
        }

        public bool PlayerEmailExists(string email)
        {
            var query = Query.EQ("Email", email.ToLower());
            var playerCollection = Dbh.GetCollection<Player>("Players");
            var player = playerCollection.FindOne(query);

            if (player != null)
            {
                return true;
            }

            return false;
        }

        public bool PlayerNameExists(string name)
        {
            var playerCollection = Dbh.GetCollection<Player>("Players");
            var query = Query.EQ("Name", name);
            var player = playerCollection.FindOne(query);

            if (player != null)
            {
                return true;
            }

            return false;
        }

    }
}