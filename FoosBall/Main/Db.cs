using System.Configuration;
using MongoDB.Driver;

namespace FoosBall.Main
{
    public static class Db
    {
        private static MongoDatabase Dbh { get; set; }
        private static string ConnectionString { get; set; }
        private static string DatabaseName { get; set; }
        private static MongoServer Server { get; set; }
        
        public static MongoDatabase GetDataBaseHandle()
        {
            // If there is no db handle then create one
            if (Dbh == null)
            {
                // initialize remote connection
                ConnectionString = ConfigurationManager.ConnectionStrings["AppHarborMongoLab"].ConnectionString;
                DatabaseName = MongoUrl.Create(ConnectionString).DatabaseName;
                Server = MongoServer.Create(ConnectionString);
                
                // if remote server responds then assume success and return handle, 
                // if not then try local server
                try
                {
                    if (Server != null) Server.Ping();
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
            } else {

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