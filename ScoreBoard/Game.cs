namespace ScoreBoard
{
    public class Game
    {
        public int Id { get; }
        public Team HomeTeam { get; }

        public Team AwayTeam { get; }

        public Game (int id, Team homeTeam, Team awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Id = id;
        }
    }
}