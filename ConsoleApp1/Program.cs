public class Program
    {
        public static void Main()
        {
            var game = new Game();

            for (int i = 0; i < 1; i++)
            {
                game.Roll(5);
                game.Roll(4);
            }

            Console.WriteLine("Total Score: " + game.Score());
        }

    }

    public class Game
    {
        private int[] rolls = new int[21];
        private int roll = 0;

        public void Roll(int pins)
        {
            rolls[roll] = pins;
            roll++;
        }

        public int Score()
        {
            int score = 0;
            int rolling = 0;

            for (int frame = 0; frame < 10; frame++)
            {
                if (rolls[rolling] == 10)
                {
                    score += 10 + rolls[rolling + 1] + rolls[rolling + 2];
                    rolling += 1;
                }
                else if (rolls[rolling] + rolls[rolling + 1] == 10)
                {
                    score += 10 + rolls[rolling + 2];
                    rolling += 2;
                }
                else
                {
                    score += rolls[rolling] + rolls[rolling + 1];
                    rolling += 2;
                }
            }

            return score;
        }
    }