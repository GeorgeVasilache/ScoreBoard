namespace ScoreBoard
{
    public class Game
    {
        public Team HomeTeam { get; }

        public Team AwayTeam { get; }

        public Game (Team homeTeam, Team awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }
    }
}