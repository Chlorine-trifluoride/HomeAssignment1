using System;

namespace HomeworkApp
{
    class Program
    {
        static void Main(string[] args)
        {
            for (; ; )
            {
                AgeVerification();
            }
        }

        static void AgeVerification()
        {
            int birthYear = GetBirthYear();

            if (birthYear == -1) // suspicious
                return;

            int currentYear = DateTime.Now.Year;

            int age = currentYear - birthYear;
            Console.WriteLine($"Your age is {age}.");

            if (age < 18)
            {
                Console.WriteLine("You are not old enough to use this application.");
            }
            else
            {
                Console.WriteLine("Welcome to the application!");
                PrintVerificationQuestions();
            }
        }

        static void PrintVerificationQuestions()
        {
            const string text = @"What was done to the VHS cassette after finishing a movie?
A. The cassette was blown into.
B. The cassette was flipped to reveal the rest of the content.
C. The cassette was rewinded to begin at the start.";

            Console.WriteLine("-------");
            Console.WriteLine(text);

            var keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.C:
                    Console.WriteLine("Welcome to the application!");
                    break;

                case ConsoleKey.A:
                case ConsoleKey.B:
                    Console.WriteLine("Nice try, kiddo..");
                    break;

                default:
                    Console.WriteLine("Invalid selection");
                    break;
            }
        }

        static int GetBirthYear()
        {
            int year = 0;
            do
            {
                Console.WriteLine("Please enter your birthyear");
                string userInput = Console.ReadLine();
                if (userInput.Length != 4)
                {
                    Console.WriteLine("Suspicious birth year....");
                    return -1;
                }

                int.TryParse(userInput, out year);
                if (year < 0) // disallow negative values
                    return -1;

            } while (year == 0);

            return year;
        }
    }
}
