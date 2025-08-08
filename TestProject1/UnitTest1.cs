namespace TestProject1
{
    public class UnitTest1
    {
        private Game _game;

        public UnitTest1()
        {
            _game = new Game();
        }

        private void RollMany(int rolls, int pins)
        {
            for (var i = 0; i < rolls; i++)
                _game.Roll(pins);
        }

        [Fact]
        public void GutterGame_AllRollsZero_ScoreIsZero()
        {
            RollMany(20, 0);
            Assert.Equal(0, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void PerfectGame_AllStrikes_ScoreIs300()
        {
            RollMany(12, 10);
            Assert.Equal(300, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void AllSpares_EachFrame5And5_LastRoll5_ScoreIs150()
        {
            RollMany(21, 5); 
            Assert.Equal(150, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void OpenFrames_NoSparesOrStrikes_ScoreSumOfPins()
        {
            RollMany(20, 3);
            Assert.Equal(60, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void SpareInFrame_ProperBonusAdded()
        {
            _game.Roll(7);
            _game.Roll(3); 
            _game.Roll(4);
            _game.Roll(2);
            RollMany(16, 0);

            Assert.Equal(20, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void StrikeInFrame_ProperBonusAdded()
        {
            _game.Roll(10); 
            _game.Roll(3);
            _game.Roll(4);
            RollMany(16, 0);

            Assert.Equal(24, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void SpareInTenthFrame_AllowsOneExtraRoll()
        {
            RollMany(18, 0);
            _game.Roll(6);
            _game.Roll(4);
            _game.Roll(7); 

            Assert.Equal(17, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void StrikeInTenthFrame_AllowsTwoExtraRolls()
        {
            RollMany(18, 0);
            _game.Roll(10);
            _game.Roll(10); 
            _game.Roll(10); 

            Assert.Equal(30, _game.Score());
            Assert.True(_game.IsComplete);
        }

        [Fact]
        public void OpenTenthFrame_NoExtraRollsAllowed()
        {
            RollMany(18, 0);
            _game.Roll(3);
            _game.Roll(5);

            Assert.True(_game.IsComplete);

            var ex = Assert.Throws<InvalidOperationException>(() => _game.Roll(1));
            Assert.Contains("No more rolls allowed", ex.Message);
        }

        [Fact]
        public void NegativePins_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _game.Roll(-1));
            Assert.Contains("Pins must be between 0 and 10", ex.Message);
        }

        [Fact]
        public void PinsMoreThan10_ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _game.Roll(11));
            Assert.Contains("Pins must be between 0 and 10", ex.Message);
        }

        [Fact]
        public void FrameSumMoreThan10_ThrowsArgumentOutOfRangeException()
        {
            _game.Roll(7);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _game.Roll(6)); // 7 + 6 > 10
            Assert.Contains("Total pins in a frame cannot exceed 10", ex.Message);
        }

        [Fact]
        public void RollingAfterGameComplete_ThrowsInvalidOperationException()
        {
            RollMany(12, 10); 

            var ex = Assert.Throws<InvalidOperationException>(() => _game.Roll(5));
            Assert.Contains("already complete", ex.Message);
        }

        [Fact]
        public void RollTooManyTimesInTenthFrame_ThrowsInvalidOperationException()
        {
            RollMany(18, 0);
            _game.Roll(10);
            _game.Roll(10);
            _game.Roll(10); 

            var ex = Assert.Throws<InvalidOperationException>(() => _game.Roll(5));
            Assert.Contains("already complete", ex.Message);
        }

        [Fact]
        public void SpareWithInvalidThirdRollInTenthFrame_ThrowsInvalidOperationException()
        {
            RollMany(18, 0);
            _game.Roll(5);
            _game.Roll(5);
            _game.Roll(7); 

            var ex = Assert.Throws<InvalidOperationException>(() => _game.Roll(3)); // no 4th roll
            Assert.Contains("already complete", ex.Message);
        }
    }
}
