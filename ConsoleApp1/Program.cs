public class Program
{
    public static void Main()
    {
        var game = new Game();

        game.Roll(10);

        game.Roll(10);

        game.Roll(4);
        game.Roll(0);

        Console.WriteLine("Total Score: " + game.Score());
    }
}

public class Game
{
    private int[] rolls = new int[21];
    private int roll = 0;
    private int frameRoll = 0; 
    private int frameCount = 0;

    public void Roll(int pins)
    {
        if (frameCount >= 10)
            return;

        rolls[roll++] = pins;

        if (frameRoll == 0 && pins == 10)
        {
            rolls[roll++] = 0;
            frameCount++;
            frameRoll = 0;
        }
        else if (frameRoll == 0)
        {
            frameRoll = 1;
        }
        else
        {
            frameRoll = 0;
            frameCount++;
        }
    }

    public int Score()
    {
        int score = 0;
        int rolling = 0;

        for (int frame = 0; frame < 10; frame++)
        {
            if (rolls[rolling] == 10)
            {
                score += 10 + rolls[rolling + 2] + rolls[rolling + 3];
                rolling += 2;
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