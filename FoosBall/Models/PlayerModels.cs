using System.Collections.Generic;
using MongoDB.Bson;

namespace FoosBall.Models
{
    public class Player : FoosBallDoc
    {
        public Player()
        {
            Rating = 1000;
            Deactivated = false;
            RememberMe = true;
        }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
       
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Played { get; set; }
        public double Rating { get; set; }
        
        public bool Deactivated { get; set; }
        public bool RememberMe { get; set; }
    }

    public class PlayerModel
    {
        public IEnumerable<Player> Players { get; set; }
    }
}