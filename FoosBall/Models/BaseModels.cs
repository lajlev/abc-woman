using MongoDB.Bson;

namespace FoosBall.Models
{
    public abstract class FoosBallDoc
    {
        public BsonObjectId Id;
    }

    public class Company
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
    }
}