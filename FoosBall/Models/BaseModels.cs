using MongoDB.Bson;

namespace FoosBall.Models
{
    public abstract class FoosBallDoc
    {
        public BsonObjectId Id;
    }

    public class FoosBallLists
    {
        public static string[] Departments =
            {
                "Administration",
                "Technology",
                "Finance",
                "Marketing",
                "Sales",
                "Support"
            };

        public static string[] Positions =
            {
                "None",
                "Defense",
                "Offense"
            };
    }
}