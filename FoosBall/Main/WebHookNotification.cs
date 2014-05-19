namespace FoosBall.Main
{
    using System;
    using System.Net;
    using System.Threading;
    using FoosBall.Models.Domain;
    using Newtonsoft.Json;

    public class WebHookNotification
    {
        public static void Send(Models.Domain.Match mongoMatch)
        {
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    try
                    {
                        var db = new Db().Dbh;
                        var config = db.GetCollection<Config>("Config").FindOne();

                        if (string.IsNullOrEmpty(config.NotificationUrl))
                        {
                            return;
                        }

                        var simpleMatch = new Match
                            {
                                CreationTime = mongoMatch.CreationTime,
                                GameOverTime = mongoMatch.GameOverTime,
                                DistributedRating = mongoMatch.DistributedRating,
                                BlueTeam = new Team
                                           {
                                               Score = mongoMatch.BlueScore,
                                               Player1 = CreatePlayer(mongoMatch.BluePlayer1),
                                               Player2 = CreatePlayer(mongoMatch.BluePlayer2)
                                           }, 
                                RedTeam = new Team
                                          {
                                              Score = mongoMatch.RedScore,
                                              Player1 = CreatePlayer(mongoMatch.RedPlayer1),
                                              Player2 = CreatePlayer(mongoMatch.RedPlayer2)
                                          }
                            };

                        var jsonMatch = JsonConvert.SerializeObject(simpleMatch, Formatting.Indented);
                        var client = new WebClient();
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.UploadString(config.NotificationUrl, jsonMatch);
                    }
                    catch
                    {
                        //Don't care, this should never disrupt anything. 
                    }
                });
        }

        private static Player CreatePlayer(Models.Domain.Player mongoPlayer)
        {
            if (mongoPlayer == null)
            {
                return null;
            }

            return new Player { Name = mongoPlayer.Name, Email = mongoPlayer.Email, Rating = mongoPlayer.Rating };
        }

        private class Match
        {
            public DateTime CreationTime { get; set; }
            public DateTime GameOverTime { get; set; }
            public double DistributedRating { get; set; }
            public Team RedTeam { get; set; }
            public Team BlueTeam { get; set; }
        }

        private class Player
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public double Rating { get; set; }
        }

        private class Team
        {
            public int Score { get; set; }
            public Player Player1 { get; set; }
            public Player Player2 { get; set; }
        }
    }
}