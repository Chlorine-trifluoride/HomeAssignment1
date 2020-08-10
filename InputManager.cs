using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeworkApp
{
    class InputManager
    {
        public int GetYear(Renderer renderer)
        {
            int result;
            bool success = false;

            do
            {
                renderer.Render();
                success = TryGetYear(out result);
            } while (!success);

            return result;
        }

        private bool TryGetYear(out int result)
        {
            string inputString = "";

            void Clear()
            {
                inputString = "";
                Renderer.Instance.Render();
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(false);

                    // are done entering the year?
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        // is the length correct?
                        if (inputString.Trim().Length == 4)
                            break;
                        else // clear and retry on incorrect input
                            Clear();
                    }

                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {   // easier to clear the whole thing on backspace
                        Clear();
                    }

                    inputString += keyInfo.KeyChar;
                }

                else
                {
                    // clear the input if the window has had changes and re-render
                    if (Renderer.Instance.CheckForResizeAndReRender())
                        inputString = "";
                }
            }

            return (int.TryParse(inputString, out result));
        }

        public int GetQuestionAnswer()
        {
            int selection = 0;
            bool done = false;

            do
            {
                // reset the keyinfo
                ConsoleKeyInfo? keyInfo = null;

                if (Console.KeyAvailable)
                    keyInfo = Console.ReadKey(true);

                switch (keyInfo?.Key)
                {
                    case ConsoleKey.UpArrow:
                        ModifySelection(ref selection, -1);
                        Renderer.Instance.SetSelectionIdAndRender(selection);
                        break;

                    case ConsoleKey.DownArrow:
                        ModifySelection(ref selection, 1);
                        Renderer.Instance.SetSelectionIdAndRender(selection);
                        break;

                    case ConsoleKey.Enter:
                        done = true;
                        break;
                }

                Renderer.Instance.CheckForResizeAndReRender();
                Thread.Sleep(16 * 2);
            } while (!done);

            return selection;
        }

        private void ModifySelection(ref int selection, int amount)
        {
            selection += amount;
            int maxLen = QuestionLoader.Instance.CurrentQuestion.Options.Length - 1;

            if (selection < 0)
                selection = maxLen;

            else if (selection > maxLen)
                selection = 0;
        }
    }
}
