using FluentAssertions;
using System.Linq;
using Xunit;

namespace ScoreBoard.Tests
{
    public class ScoreBoardTests
    {
        [Theory]
        [InlineData("Mexico", "Canada")]
        [InlineData("Spain", "Brazil")]
        [InlineData("Germany", "France")]
        [InlineData("Uruguay", "Italy")]
        [InlineData("Argentina", "Australia")]
        public void StartGame_SingleMatch_StoresInitialMatchData(string homeTeamName, string awayTeamName)
        {
            ScoreBoard scoreBoard = new ScoreBoard();
            
            scoreBoard.StartGame(homeTeamName, awayTeamName);

            Game currentGame = scoreBoard.GetSummary().Single();

            Game expectedGame = new Game(new Team(homeTeamName), new Team(awayTeamName));

            currentGame.Should().BeEquivalentTo(expectedGame);
        }
    }
}
