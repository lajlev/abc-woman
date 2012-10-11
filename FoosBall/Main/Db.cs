using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MongoDB.Driver;

namespace FoosBall.Main
{
    public static class Db
    {
        public static MongoDatabase GetDataBaseHandle()
        {
            if (Dbh == null)
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["AppHarborMongoHQ"].ConnectionString;
                var connectionString = ConfigurationManager.ConnectionStrings["AppHarborMongoLab"].ConnectionString;
                var _databaseName = MongoUrl.Create(connectionString).DatabaseName;
                var server = MongoServer.Create(connectionString);
                // server.Ping();
                Dbh = server.GetDatabase(_databaseName);   
            }
            return Dbh;
        }

        private static MongoDatabase Dbh { get; set; }
    }
}