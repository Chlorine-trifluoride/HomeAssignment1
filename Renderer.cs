using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace HomeworkApp
{
    enum RENDERMODE
    {
        UNKNOWN = 0,
        AGE_VERIFICATION,
        QUESTION,
        MESSAGE,
        SLOTS,
        MAINMENU,
        OTHER
    }

    sealed class Renderer
    {
        public static Renderer Instance { get; } = new Renderer();

        public RENDERMODE Rendermode { get; set; }
        public string MessageToRender { get; set; }
        public string SlotsToRender { get; set; }
        public string WinningsToRender { get; set; }
        public string PointsToRender { get; set; }
        public MenuOptions MenuScreenOptions { get; set; }

        private Action[] renderingFuncPtrs;
        private int width, height;
        private int selectionId = 0;

        private int halfWidth => Console.WindowWidth / 2;

        private Renderer()
        {
            UpdateWindowDimensions();

            renderingFuncPtrs = new Action[]
            {
                RenderUnknown,
                RenderAge,
                RenderQuestion,
                RenderMessage,
                RenderSlots,
                RenderMenu,
                RenderOther
            };
        }

        private void UpdateWindowDimensions()
        {
            this.width = Console.WindowWidth;
            this.height = Console.WindowHeight;
        }

        public void Render()
        {
            Console.Clear();
            SetColorNormal();
            renderingFuncPtrs[(int)Rendermode].Invoke();
        }

        // returns true if window has changed
        public bool CheckForResizeAndReRender()
        {
            if (Console.WindowWidth != width &&
                Console.WindowHeight != height)
            {
                UpdateWindowDimensions();
                Render();
                return true;
            }

            return false;
        }

        private void WriteCentered(string text, int top)
        {
            int halfText = text.Length / 2;
            int centered = halfWidth - halfText;

            // TODO: proper error handling
            WriteMessageAtSafe(centered, top, text);
        }

        private void WriteCentered(string text)
        {
            WriteCentered(text, Console.CursorTop);
        }

        public void SetSelectionIdAndRender(int id)
        {
            selectionId = id;
            Render();
        }

        private void SetColorNormal()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        private void SetColorHighlight()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
        }

        private void WriteMessageAtSafe(int left, int top, string message)
        {
            if (left < 0 || top < 0 && Console.BufferWidth < left || Console.BufferHeight < top)
            {
                const string errorText = "Window too small";
                if (Console.BufferWidth > errorText.Length)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write(errorText);
                }
            }

            else // the text should fit
            {
                Console.SetCursorPosition(left, top);
                Console.Write(message);
            }
        }

        private void SetCursorAtSafe(int left, int top)
        {
            if (left < 0 || top < 0 && Console.BufferWidth < left || Console.BufferHeight < top)
                Console.SetCursorPosition(0, 0);

            else
                Console.SetCursorPosition(left, top);
        }

        private void RenderUnknown()
        {
            throw new NotImplementedException();
        }

        private void RenderAge()
        {
            WriteCentered("Enter your birth year", 2);

            WriteCentered("=====================", 3);

            WriteCentered("=====================", 5);

            SetCursorAtSafe(halfWidth - 2, 4);
        }

        private void RenderQuestion()
        {
            Question question = QuestionLoader.Instance.CurrentQuestion;

            WriteCentered("=====================", 2);
            WriteCentered(question.Title, 3);
            WriteCentered("=====================", 4);

            for (int i = 0; i < question.Options.Length; i++)
            {
                if (selectionId == i)
                    SetColorHighlight();
                else
                    SetColorNormal();

                WriteCentered(question.Options[i], 6 + i);
            }

            SetColorNormal();

            // Render Help
            WriteMessageAtSafe(1, Console.WindowHeight - 2, "Use arrow keys and enter to select your answer.");
        }

        private void RenderMessage()
        {
            WriteCentered("Message:", 2);

            WriteCentered("=====================", 3);

            WriteCentered("=====================", 5);

            WriteCentered(MessageToRender, 4);
        }

        private void RenderSlots()
        {
            WriteCentered("Your Symbols:", 2);

            WriteCentered("=====================", 3);
            WriteCentered(SlotsToRender, 4);
            WriteCentered("=====================", 5);
            WriteCentered(PointsToRender, 6);
            WriteCentered("============", 7);
            WriteCentered(WinningsToRender, 8);
        }

        private void RenderMenu()
        {
            WriteCentered("=====================", 2);
            WriteCentered(MenuScreenOptions.Title, 3);
            WriteCentered("=====================", 4);

            for (int i = 0; i < MenuScreenOptions.MenuItems.Length; i++)
            {
                if (selectionId == i)
                    SetColorHighlight();
                else
                    SetColorNormal();

                WriteCentered(MenuScreenOptions.MenuItems[i].Text, 6 + i);
            }

            SetColorNormal();

            // Render Help
            WriteMessageAtSafe(1, Console.WindowHeight - 2, "Use arrow keys and enter to select your answer.");
        }

        private void RenderOther()
        {
            throw new NotImplementedException();
        }
    }
}
