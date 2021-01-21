using System.Collections.Generic;

namespace ScoreBoard
{
    public class ScoreBoard
    {
        private List<Game> games = new List<Game>();

        public void StartGame(string homeTeam, string awayTeam)
        {
            games.Add(new Game(new Team(homeTeam), new Team(awayTeam)));
;        }

        public List<Game> GetSummary()
        {
            return games;
        }
    }
}