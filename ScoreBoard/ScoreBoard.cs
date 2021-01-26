using System;
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

        public void FinishGame(int gameId)
        {
            games.Remove(games.Find(g => g.Id == gameId));
        }

        public void UpdateScore(int gameId, int homeTeamScore, int awayTeamScore)
        {
            Game game = games.Find(g => g.Id == gameId);

            if (game == null) throw new InvalidOperationException("Game not found");

            game.Update(homeTeamScore, awayTeamScore);

            games.Sort();
        }

        public List<Game> GetSummary()
        {
            return games;
        }
    }
}