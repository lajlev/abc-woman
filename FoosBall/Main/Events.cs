namespace FoosBall.Main
{
    using System;

    using FoosBall.Models;

    using Microsoft.AspNet.SignalR;

    using MongoDB.Bson;

    public static class Events
    {
        public static void Say(string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<Chat>();
            context.Clients.All.addMessage(message);
        }

        public static void SubmitEvent(string action, string type, object targetObject, BsonObjectId userId)
        {
            Say(action + " " + type + " was fired.");
            SaveEvent(action, type, targetObject, userId);
        }

        private static void SaveEvent(string action, string type, object targetObject, BsonObjectId userId)
        {
            if (string.IsNullOrEmpty(action) == false && string.IsNullOrEmpty(type) == false && targetObject != null)
            {
                var dbh = new Db(AppConfig.GetEnvironment()).Dbh;
                var eventCollection = dbh.GetCollection<Event>("Events");

                var newEvent = new Event
                {
                    Action = action,
                    Type = type,
                    Created = new BsonDateTime(DateTime.Now),
                    CreatedBy = userId
                };

                if (type.ToLower() == "player")
                {
                    newEvent.Player = (Player)targetObject;
                }

                if (type.ToLower() == "match")
                {
                    newEvent.Match = (Match)targetObject;
                }

                eventCollection.Insert(newEvent);
            }
        }
    }
}