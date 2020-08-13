using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace HomeworkApp
{
    class Application1 : ApplicationBase
    {

        protected override Age AgeVerification()
        {
            renderer.Rendermode = RENDERMODE.AGE_VERIFICATION;
            int birthYear = inputMgr.GetYear(renderer, InvalidYearInput);
            int currentYear = DateTime.Now.Year;
            Age age = new Age { Years = currentYear - birthYear };

            return age;
        }

        public void InvalidYearInput()
        {
            DisplayMessageToUser("Invalid year input", 1000);
            // restore the rendermode
            renderer.Rendermode = RENDERMODE.AGE_VERIFICATION;
        }

        protected override void PrintAgeReply(Age age)
        {
            // Pattern matching madness. 
            // Should be fixed in C# 9, sorry =(
            string message = age switch
            {
                { } i when i.Years < 0 => "You can not be negative years old",
                { } i when i.Years == 0 => "You are zero years old",
                { } i when i.Years < 18 => "You are too young for this application",
                { } i when i.Years > 120 => "You are suspiciously old",
                { } i when i.Years >= 18 => i.SetValid(),
                _ => throw new NotImplementedException(),
            };

            DisplayMessageToUser(message, 1000);
        }

        protected override void DisplayMessageToUser(string message, int milliseconds)
        {
            renderer.Rendermode = RENDERMODE.MESSAGE;
            renderer.MessageToRender = message;
            renderer.Render();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                // HACK: eat all keystrokes while displaying messages
                if (Console.KeyAvailable)
                    Console.ReadKey(true);

                renderer.CheckForResizeAndReRender();
                Thread.Sleep(16*2);
            } while (sw.ElapsedMilliseconds < milliseconds);

            sw.Stop();
        }

        protected override void PresentRandomQuestion()
        {
            Random random = new Random();
            int questionId = random.Next(0, QuestionLoader.Instance.Questions.Length);

            QuestionLoader.Instance.CurrentQuestion = QuestionLoader.Instance.Questions[questionId];
            renderer.Rendermode = RENDERMODE.QUESTION;
            renderer.SetSelectionIdAndRender(0); // reset selection

            int answerId = inputMgr.GetQuestionAnswer();

            if (answerId == QuestionLoader.Instance.CurrentQuestion.Answer)
                DisplayMessageToUser("Correct Answer", 1500);
            else
                DisplayMessageToUser("Wrong Answer", 1500);
        }

        /*
         * Create a .NET C# console application that draws symbols from three lists.
         * Each list contains a certain number of symbols A, K, Q and J, in a random order
         * (you can make an algorithm which randomizes the lists or just manually 
         * initialize the lists with the symbols in a seemingly random order).
         * The player starts with a balance of 20. Each time the player presses enter,
         * the balance is decreased by one, and a symbol from each list is randomly selected.
         * If all the symbols match, the player wins the amount specified in the table below:
         */
        protected override void SlotsGame()
        {
            SlotsGame slotsGame = new SlotsGame();

            slotsGame.Bet = GetBetFromBetSelectionMenu();

            // Return option returns -1
            if (slotsGame.Bet == -1)
                return;

            DisplayMessageToUser("Your cards have been shuffled", 1500);
            DisplayMessageToUser("Press Enter to draw your slots", 500);

            do
            {
                inputMgr.WaitEnter();

                (char[] slots, int winnings) drawResult = slotsGame.Draw();

                renderer.SlotsToRender = String.Join(',', drawResult.slots);
                renderer.PointsToRender = $"Points: {slotsGame.Points}";
                renderer.WinningsToRender = $"Winnings: {drawResult.winnings}";

                renderer.Rendermode = RENDERMODE.SLOTS;
                renderer.Render();
            } while (slotsGame.SymbolsRemaining > 0);

            DisplayMessageToUser("Game Over: Out of cards", 1000);
        }

        protected override int GetBetFromBetSelectionMenu()
        {
            MenuBuilder menuBuilder = new MenuBuilder();
            menuBuilder.AddTitle("Select your bet");

            // This is a messy 'temporary' solution
            int betValue = 0;

            menuBuilder.AddItem("Bet 1", ()=>betValue=1);
            menuBuilder.AddItem("Bet 5", ()=>betValue=5);
            menuBuilder.AddItem("Bet 10", ()=>betValue=10);
            menuBuilder.AddItem("Return", ()=>betValue=-1);
            menuBuilder.SetSelection(0);

            MenuOptions menuOptions = menuBuilder.Build();

            renderer.MenuScreenOptions = menuOptions;
            renderer.Rendermode = RENDERMODE.MAINMENU;

            // inputMgr will call render
            int selection = inputMgr.GetSelection(menuOptions);
            // Invoke the callback action from the MenuItem which we have selected
            menuOptions.MenuItems[selection].Callback.Invoke();

            return betValue;
        }

        protected override void MainMenu()
        {
            // Using a menu builder simplifies our logic
            MenuBuilder menuBuilder = new MenuBuilder();
            menuBuilder.AddTitle("Select an action.");
            menuBuilder.SetSelection(0);
            menuBuilder.AddItem("Questions", PresentRandomQuestion);
            menuBuilder.AddItem("Slots Game", SlotsGame);
            menuBuilder.AddItem("Exit", ExitProgram);

            // Returns the actual menu object
            MenuOptions menuOptions = menuBuilder.Build();

            renderer.MenuScreenOptions = menuOptions;
            renderer.Rendermode = RENDERMODE.MAINMENU;

            // inputMgr will call render
            int selection = inputMgr.GetSelection(menuOptions);
            // Invoke the callback action from the MenuItem which we have selected
            menuOptions.MenuItems[selection].Callback.Invoke();
        }
    }
}
