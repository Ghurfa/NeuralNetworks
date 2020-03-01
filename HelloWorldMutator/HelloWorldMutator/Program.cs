using System;

namespace HelloWorldMutator
{
    class Program
    {
        static string mutate(string input, Random random)
        {
            int charIndex = random.Next(input.Length);
            char[] inputAsArr = input.ToCharArray();
            if(inputAsArr[charIndex] == (char)32)
            {
                inputAsArr[charIndex]++;
            }
            else if (inputAsArr[charIndex] == (char)126 || random.Next(2) == 0)
            {
                inputAsArr[charIndex]--;
            }
            else
            {
                inputAsArr[charIndex]++;
            }
            return new string(inputAsArr);
        }
        static float calcMAE(string input, string goal)
        {
            int sum = 0;
            for(int i = 0; i < goal.Length; i++)
            {
                sum += Math.Abs(input[i] - goal[i]);
            }
            return (float)sum / (float)goal.Length;
        }
        static void Main(string[] args)
        {
            string goalString = "Hello, world!";
            Random random = new Random();
            char[] inputAsArr = new char[goalString.Length];
            for(int i = 0; i < goalString.Length; i++)
            {
                inputAsArr[i] = (char)random.Next(26, 127);
            }
            string input = new string(inputAsArr);
            do
            {
                float MAE = calcMAE(input, goalString);
                string mutatedInput = mutate(input, random);
                float newMAE = calcMAE(mutatedInput, goalString);
                if (MAE > newMAE)
                {
                    input = mutatedInput;
                    MAE = newMAE;
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.CursorLeft = 0;
                Console.Write($"{input} => ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{goalString} ({MAE.ToString("0.00")}) ");
            } while (input != goalString);
            Console.ReadKey();
        }
    }
}
