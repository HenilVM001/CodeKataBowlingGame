namespace TestProject1
{
    public class UnitTest1
    {
        
            [Fact]
            public void AllOpenFrames_ReturnsCorrectScore()
            {
                var game = new Game();

                for (int i = 0; i < 10; i++)
                {
                    game.Roll(5);
                    game.Roll(4);
                }

                Assert.Equal(90, game.Score());
            }

            [Fact]
            public void AllStrikes_PerfectGame_Returns300()
            {
                var game = new Game();

                for (int i = 0; i < 2; i++)
                {
                    game.Roll(10);
                }

                Assert.Equal(30, game.Score());
            }

            [Fact]
            public void SpareFollowedByThree_ReturnsCorrectScore()
            {
                var game = new Game();

                game.Roll(5);
                game.Roll(5);
                game.Roll(3);
                game.Roll(3); 

                Assert.Equal(19, game.Score()); 
            }

            [Fact]
            public void StrikeFollowedByThreeAndFour_ReturnsCorrectScore()
            {
                var game = new Game();

                game.Roll(10);
                game.Roll(3);
                game.Roll(4);

                Assert.Equal(24, game.Score());
            }

        }
    }