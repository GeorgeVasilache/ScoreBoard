using System.Collections.Generic;

namespace ScoreBoard
{
    public class ScoreBoard
    {
        private List<Game> games = new List<Game>();

        public int StartGame(string homeTeam, string awayTeam)
        {
            Game game = new Game(games.Count, new Team(homeTeam), new Team(awayTeam));

            games.Add(game);

            return game.Id;
        }

        public List<Game> GetSummary()
        {
            return games;
        }
    }
}