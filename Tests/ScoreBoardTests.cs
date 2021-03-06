﻿using FluentAssertions;
using System.Collections.Generic;
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
        public void StartGame_SingleGame_StoresInitialGameData(string homeTeamName, string awayTeamName)
        {
            ScoreBoard scoreBoard = new ScoreBoard();
            
            int gameId = scoreBoard.StartGame(homeTeamName, awayTeamName);

            Game currentGame = scoreBoard.GetSummary().Single();

            Game expectedGame = new Game(gameId, new Team(homeTeamName), new Team(awayTeamName));

            currentGame.Should().BeEquivalentTo(expectedGame);
        }

        [Fact]
        public void StartGame_MultipleGames_StoresAllGamesData()
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            List<int> ids = new List<int>();

            ids.Add(scoreBoard.StartGame("Mexico", "Canada"));
            ids.Add(scoreBoard.StartGame("Spain", "Brazil"));
            ids.Add(scoreBoard.StartGame("Germany", "France"));

            List<Game> currentGames = scoreBoard.GetSummary();

            List<Game> expectedGames = new List<Game>
            {
                new Game(ids[0], new Team("Mexico"), new Team("Canada")),
                new Game(ids[1], new Team("Spain"), new Team("Brazil")),
                new Game(ids[2], new Team("Germany"), new Team("France")),
            };

            currentGames.Should().BeEquivalentTo(expectedGames);
        }

        [Fact]
        public void FinishGame_SingleGame_NoGamesLeft()
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            int gameId = scoreBoard.StartGame("Mexico", "Canada");

            scoreBoard.FinishGame(gameId);

            scoreBoard.GetSummary().Should().BeEmpty();
        }

        [Fact]
        public void FinishGame_MultipleGames_OtherGamesLeft()
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            List<int> ids = new List<int>();

            ids.Add(scoreBoard.StartGame("Mexico", "Canada"));
            ids.Add(scoreBoard.StartGame("Spain", "Brazil"));
            ids.Add(scoreBoard.StartGame("Germany", "France"));

            scoreBoard.FinishGame(ids[1]);

            List<Game> expectedGames = new List<Game>
            {
                new Game(ids[0], new Team("Mexico"), new Team("Canada")),
                new Game(ids[2], new Team("Germany"), new Team("France")),
            };

            scoreBoard.GetSummary().Should().BeEquivalentTo(expectedGames);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(10, 2)]
        [InlineData(2, 2)]
        [InlineData(6, 6)]
        [InlineData(3, 1)]
        public void UpdateScore_SingleGame_UpdatesTeamsScore(int homeTeamScore, int awayTeamScore)
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            int gameId = scoreBoard.StartGame("Mexico", "Canada");

            scoreBoard.UpdateScore(gameId, homeTeamScore, awayTeamScore);

            scoreBoard.GetSummary().Single().HomeTeam.Score.Should().Be(homeTeamScore);
            scoreBoard.GetSummary().Single().AwayTeam.Score.Should().Be(awayTeamScore);
        }

        [Fact]
        public void UpdateScore_MultipleGames_UpdatesTeamsScore()
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            scoreBoard.StartGame("Spain", "Brazil");
            scoreBoard.StartGame("Mexico", "Canada");

            int gameId = scoreBoard.StartGame("Germany", "France");

            scoreBoard.UpdateScore(gameId, 2, 2);

            List<Game> expectedGames = new List<Game>
            {
                new Game(2, new Team("Germany") { Score = 2 }, new Team("France") { Score = 2 }),
                new Game(0, new Team("Spain"), new Team("Brazil")),
                new Game(1, new Team("Mexico"), new Team("Canada")),
            };

            expectedGames.Find(g => g.HomeTeam.Name == "Germany").Update(2, 2);

            scoreBoard.GetSummary().Should().BeEquivalentTo(expectedGames);
        }

        [Fact]
        public void GetSummary_DifferentTotalScore_ReturnsFirstTheBiggestTotalScore()
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            int mexicoGameId = scoreBoard.StartGame("Mexico", "Canada");
            int spainGameId = scoreBoard.StartGame("Spain", "Brazil");

            scoreBoard.UpdateScore(mexicoGameId, 0, 5);
            scoreBoard.UpdateScore(spainGameId, 10, 2);

            List<Game> expectedGames = new List<Game>
            {
                 new Game(spainGameId, new Team("Spain"), new Team("Brazil")),
                 new Game(mexicoGameId, new Team("Mexico"), new Team("Canada")),
            };

            expectedGames.Find(g => g.HomeTeam.Name == "Spain").Update(10, 2);
            expectedGames.Find(g => g.HomeTeam.Name == "Mexico").Update(0, 5);

            scoreBoard.GetSummary().Should().BeEquivalentTo(expectedGames, options => options.WithStrictOrdering());
        }

        [Fact]
        public void GetSummary_SameTotalScore_ReturnsFirstTheLatestGameStarted()
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            int mexicoGameId = scoreBoard.StartGame("Mexico", "Canada");
            int spainGameId = scoreBoard.StartGame("Spain", "Brazil");

            scoreBoard.UpdateScore(mexicoGameId, 5, 2);
            scoreBoard.UpdateScore(spainGameId, 5, 2);

            List<Game> expectedGames = new List<Game>
            {
                 new Game(spainGameId, new Team("Spain"), new Team("Brazil")),
                 new Game(mexicoGameId, new Team("Mexico"), new Team("Canada")),
            };

            expectedGames.Find(g => g.HomeTeam.Name == "Spain").Update(5, 2);
            expectedGames.Find(g => g.HomeTeam.Name == "Mexico").Update(5, 2);

            scoreBoard.GetSummary().Should().BeEquivalentTo(expectedGames, options => options.WithStrictOrdering());
        }

        [Fact]
        public void GetSummary_MultipleTotalScores_ReturnsGamesOrderedByTotalScore()
        {
            ScoreBoard scoreBoard = new ScoreBoard();

            int mexicoGameId = scoreBoard.StartGame("Mexico", "Canada");
            int spainGameId = scoreBoard.StartGame("Spain", "Brazil");
            int germanyGameId = scoreBoard.StartGame("Germany", "France");
            int uruguayGameId = scoreBoard.StartGame("Uruguay", "Italy");
            int argentinaGameId = scoreBoard.StartGame("Argentina", "Australia");

            scoreBoard.UpdateScore(mexicoGameId, 0, 5);
            scoreBoard.UpdateScore(spainGameId, 10, 2);
            scoreBoard.UpdateScore(germanyGameId, 2, 2);
            scoreBoard.UpdateScore(uruguayGameId, 6, 6);
            scoreBoard.UpdateScore(argentinaGameId, 3, 1);

            List<Game> expectedGames = new List<Game>
            {
                 new Game(uruguayGameId, new Team("Uruguay"), new Team("Italy")),
                 new Game(spainGameId, new Team("Spain"), new Team("Brazil")),
                 new Game(mexicoGameId, new Team("Mexico"), new Team("Canada")),
                 new Game(argentinaGameId, new Team("Argentina"), new Team("Australia")),
                 new Game(germanyGameId, new Team("Germany"), new Team("France")),
            };

            expectedGames.Find(g => g.HomeTeam.Name == "Uruguay").Update(6, 6);
            expectedGames.Find(g => g.HomeTeam.Name == "Spain").Update(10, 2);
            expectedGames.Find(g => g.HomeTeam.Name == "Mexico").Update(0, 5);
            expectedGames.Find(g => g.HomeTeam.Name == "Argentina").Update(3, 1);
            expectedGames.Find(g => g.HomeTeam.Name == "Germany").Update(2, 2);

            scoreBoard.GetSummary().Should().BeEquivalentTo(expectedGames, options => options.WithStrictOrdering());
        }
    }
}
