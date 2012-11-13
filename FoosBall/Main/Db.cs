namespace FoosBall.Main
{
    using System.Configuration;
    using FoosBall.Models.Base;
    using MongoDB.Driver;
    
    public class Db
    {
        public Db(Environment environment = Environment.Production)
        {
            // Determine environment
            if (environment == Environment.Production)
            {
                this.ConnectionString = ConfigurationManager.ConnectionStrings["FoosBall.MongoLab"].ConnectionString;
            }

            if (environment == Environment.Staging)
            {
                this.ConnectionString = ConfigurationManager.ConnectionStrings["FoosBallStaging.MongoLab"].ConnectionString;
            }

            if (environment == Environment.Local)
            {
                this.ConnectionString = ConfigurationManager.ConnectionStrings["Local.MongoDb"].ConnectionString;
            }

            // Try to connect to server
            this.DatabaseName = MongoUrl.Create(this.ConnectionString).DatabaseName;
            this.Server = MongoServer.Create(this.ConnectionString);

            if (this.Server != null)
            {
                // if remote server responds then assume success and return handle, 
                // if not then try local server
                try
                {
                    this.Server.Ping();
                }
                catch
                {
                    this.ConnectionString = ConfigurationManager.ConnectionStrings["Local.MongoDb"].ConnectionString;
                    if (this.ConnectionString != null)
                    {
                        this.DatabaseName = MongoUrl.Create(this.ConnectionString).DatabaseName;
                        this.Server = MongoServer.Create(this.ConnectionString);
                    }
                }

                this.Dbh = this.Server.GetDatabase(this.DatabaseName);
            }
        }

        public MongoDatabase Dbh { get; set; }

        private string ConnectionString { get; set; }

        private string DatabaseName { get; set; }

        private MongoServer Server { get; set; }

    }
}