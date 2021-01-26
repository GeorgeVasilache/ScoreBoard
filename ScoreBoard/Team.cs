namespace ScoreBoard
{
    public class Team
    {
        public string Name { get; }

        public int Score { get; set; }

        public Team(string name)
        {
            Name = name;
        }
    }
}
