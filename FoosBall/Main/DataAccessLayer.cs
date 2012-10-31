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

        public static string GetMostPlayed(MongoCollection<Player> playerCollection)
        {
            var orderedList = playerCollection.Find(Query.Exists("Played")).SetSortOrder(SortBy.Descending("Played"));
            var numberOfPlayed = orderedList.First().Played;
            var sb = new StringBuilder();
            var mostPlayed = string.Empty;

            foreach (var player in orderedList)
            {
            }

            return string;
        }
    }

}