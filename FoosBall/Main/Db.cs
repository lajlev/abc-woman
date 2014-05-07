namespace FoosBall.Main
{
    using System.Configuration;
    using FoosBall.Models.Base;
    using MongoDB.Driver;
    
    public class Db
    {
        public Db(Environment environment = Environment.Production)
        {
            // Try to connect to server
            this.ConnectionString = ConfigurationManager.AppSettings["Database"];
            this.DatabaseName = MongoUrl.Create(this.ConnectionString).DatabaseName;
            this.Server = MongoServer.Create(this.ConnectionString);

            if (this.Server != null)
            {
                this.Dbh = this.Server.GetDatabase(this.DatabaseName);
            }
        }

        public MongoDatabase Dbh { get; set; }

        private string ConnectionString { get; set; }

        private string DatabaseName { get; set; }

        private MongoServer Server { get; set; }
    }
}