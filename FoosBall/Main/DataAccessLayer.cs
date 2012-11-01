namespace FoosBall.Main
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using FoosBall.Models;

    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    public class DataAccessLayer
    {
        protected readonly MongoDatabase Dbh;

        public DataAccessLayer()
        {
            this.Dbh = Db.GetDataBaseHandle();
        }

    }

}