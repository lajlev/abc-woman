namespace FoosBall.Models
{
    using System;

    using FoosBall.Models.Base;

    using MongoDB.Bson;

    public class Player : FoosBallDoc
    {
        public Player()
        {
            Rating = 1000;
            Deactivated = false;
            RememberMe = true;
            Won = 0;
            Lost = 0;
            Played = 0;
            Created = new BsonDateTime(DateTime.Now);
        }
            
        public string Email { get; set; }

        public string Name { get; set; }
        
        public string NickName { get; set; }
        
        public string Password { get; set; }
        
        public string Department { get; set; }
        
        public string Position { get; set; }
       
        public int Won { get; set; }
        
        public int Lost { get; set; }
        
        public int Played { get; set; }
        
        public double Rating { get; set; }
        
        public bool Deactivated { get; set; }
        
        public bool RememberMe { get; set; }
    }
}