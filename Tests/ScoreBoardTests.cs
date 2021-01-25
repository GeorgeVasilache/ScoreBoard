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
    }
}
