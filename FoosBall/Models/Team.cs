namespace FoosBall.Models
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class Team
    {
        public Team()
        {
            this.MatchTeam = new List<Player>();
        }
        
        public List<Player> MatchTeam { get; set; }
        
        public double GetTeamRating()
        {
            return this.MatchTeam.Sum(player => player.Rating);
        }
    }
}