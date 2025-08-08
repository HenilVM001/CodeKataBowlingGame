public class Program
{
    public static void Main()
    {
        var game = new Game();

        Console.WriteLine("Enter pins knocked down for each roll (enter 'q' to finish):");

        while (!game.IsComplete)
        {
            Console.Write("Roll: ");
            var input = Console.ReadLine();

            if (input?.Trim().ToLower() == "q")
            {
                break;
            }

            if (int.TryParse(input, out var pins))
            {
                try
                {
                    game.Roll(pins);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("Invalid roll: " + ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("Game complete. No more rolls allowed.");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number between 0 and 10, or 'q' to quit.");
            }
        }

        Console.WriteLine("Total Score: " + game.Score());
    }
}

public class Game
{
    private readonly int[] _rolls = new int[21];
    private int _currentRoll = 0;

    public bool IsComplete { get; private set; } = false;

    public void Roll(int pins)
    {
        if (IsComplete)
            throw new InvalidOperationException("The game is already complete.");

        if (pins < 0 || pins > 10)
            throw new ArgumentOutOfRangeException(nameof(pins), "Pins must be between 0 and 10.");

        ValidatePinsForCurrentRoll(pins);

        _rolls[_currentRoll++] = pins;

        if (_currentRoll >= 12 && CalculateFramesCompleted() >= 10)
        {
            if (IsTenthFrameComplete())
            {
                IsComplete = true;
            }
        }
    }

    public int Score()
    {
        var score = 0;
        var rollIndex = 0;

        for (var frame = 0; frame < 10; frame++)
        {
            if (IsStrike(rollIndex))
            {
                score += 10 + StrikeBonus(rollIndex);
                rollIndex++;
            }
            else if (IsSpare(rollIndex))
            {
                score += 10 + SpareBonus(rollIndex);
                rollIndex += 2;
            }
            else
            {
                score += SumOfBallsInFrame(rollIndex);
                rollIndex += 2;
            }
        }

        return score;
    }

    private void ValidatePinsForCurrentRoll(int pins)
    {
        var rollIndex = _currentRoll;

        if (CalculateFramesCompleted() == 9)
        {
            if (rollIndex >= 20)
                throw new InvalidOperationException("No more rolls allowed in the 10th frame.");

            if (rollIndex == 19)
            {
                if (_rolls[18] != 10 && _rolls[18] + pins > 10)
                    throw new ArgumentOutOfRangeException(nameof(pins), "Total pins in 10th frame's first two rolls cannot exceed 10 unless first roll was a strike.");
            }

            if (rollIndex == 20)
            {
                var first = _rolls[18];
                var second = _rolls[19];
                if (first != 10 && first + second != 10)
                    throw new InvalidOperationException("No third roll allowed unless spare or strike in 10th frame.");
            }

            return;
        }

        if (rollIndex % 2 == 1)
        {
            if (_rolls[rollIndex - 1] + pins > 10)
                throw new ArgumentOutOfRangeException(nameof(pins), "Total pins in a frame cannot exceed 10.");
        }
    }

    private int CalculateFramesCompleted()
    {
        var frameCount = 0;
        var rollIndex = 0;

        while (frameCount < 10 && rollIndex < _currentRoll)
        {
            if (_rolls[rollIndex] == 10)
                rollIndex++;
            else
                rollIndex += 2;

            frameCount++;
        }

        return frameCount;
    }

    private bool IsTenthFrameComplete()
    {
        var roll18 = _rolls[18];
        var roll19 = _rolls[19];
        var roll20 = _rolls[20];

        if (roll18 == 10)
        {
            return _currentRoll >= 21;
        }
        else if (roll18 + roll19 == 10)
        {
            return _currentRoll >= 21;
        }
        else
        {
            return _currentRoll >= 20;
        }
    }

    private bool IsStrike(int rollIndex)
    {
        return _rolls[rollIndex] == 10;
    }

    private bool IsSpare(int rollIndex)
    {
        return _rolls[rollIndex] + _rolls[rollIndex + 1] == 10;
    }

    private int StrikeBonus(int rollIndex)
    {
        return _rolls[rollIndex + 1] + _rolls[rollIndex + 2];
    }

    private int SpareBonus(int rollIndex)
    {
        return _rolls[rollIndex + 2];
    }

    private int SumOfBallsInFrame(int rollIndex)
    {
        return _rolls[rollIndex] + _rolls[rollIndex + 1];
    }
}
