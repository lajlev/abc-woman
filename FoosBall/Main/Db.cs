namespace FoosBall.Main
{
    using System.Configuration;
    using FoosBall.Models.Base;
    using MongoDB.Driver;
    
    public static class Db
    {
        private static MongoDatabase Dbh { get; set; }

        private static string ConnectionString { get; set; }

        private static string DatabaseName { get; set; }

        private static MongoServer Server { get; set; }
        
        public static MongoDatabase GetDataBaseHandle(Environment environment = Environment.Production)
        {
            // If there is no db handle then create one
            if (Dbh == null)
            {
                // initialize remote connection
                if (environment == Environment.Production)
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["FoosBall.MongoLab"].ConnectionString;                    
                }
                if (environment == Environment.Staging)
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["AppHarborMongoLab"].ConnectionString;                    
                }
                if (environment == Environment.Local)
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["LocalMongoDb"].ConnectionString;                    
                }
                DatabaseName = MongoUrl.Create(ConnectionString).DatabaseName;
                Server = MongoServer.Create(ConnectionString);
                
                if (Server != null)
                {
                    // if remote server responds then assume success and return handle, 
                    // if not then try local server
                    try
                    {
                        Server.Ping();
                    }
                    catch
                    {
                        ConnectionString = ConfigurationManager.ConnectionStrings["LocalMongoDb"].ConnectionString;
                        if (ConnectionString != null)
                        {
                            DatabaseName = MongoUrl.Create(ConnectionString).DatabaseName;
                            Server = MongoServer.Create(ConnectionString);
                        }
                    }
                    
                    Dbh = Server.GetDatabase(DatabaseName);
                }
            }
            else
            {
                // If there IS already a db handle then check if it's responding
                // If not then try to create new connection
                try
                {
                    Server.Ping();
                }
                catch
                {
                    Dbh = null;
                    GetDataBaseHandle();
                }
            }
            
            return Dbh;
        }
    }
}