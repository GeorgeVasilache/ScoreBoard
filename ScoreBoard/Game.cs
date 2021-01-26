namespace ScoreBoard
{
    public class Game
    {
        public int Id { get; }
        public Team HomeTeam { get; }

        public Team AwayTeam { get; }

        public int TotalScore { get; private set; }

        public Game (int id, Team homeTeam, Team awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Id = id;
        }

        public void Update(int homeTeamScore, int awayTeamScore)
        {
            HomeTeam.Score = homeTeamScore;
            AwayTeam.Score = awayTeamScore;

            TotalScore = homeTeamScore + awayTeamScore;
        }
    }
}