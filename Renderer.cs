using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        OTHER
    }

    sealed class Renderer
    {
        public static Renderer Instance { get; } = new Renderer();

        public RENDERMODE Rendermode { get; set; }
        public string MessageToRender { get; set; }
        private Action[] functionPointers;
        private int width, height;
        private int selectionId = 0;

        private Renderer()
        {
            UpdateWindowDimensions();

            functionPointers = new Action[]
            {
                RenderUnknown,
                RenderAge,
                RenderQuestion,
                RenderMessage,
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
            functionPointers[(int)Rendermode].Invoke();
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
            int w = Console.WindowWidth;
            int hw = w / 2;
            int halfText = text.Length / 2;
            int centered = hw - halfText;

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
                    Console.Write("Window too small");
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

        }

        private void RenderAge()
        {
            int w = Console.WindowWidth;
            int h = Console.WindowHeight;
            int halfW = w / 2;
            int halfH = h / 2;

            Console.Clear();

            WriteCentered("Enter your birth year", 2);

            WriteCentered("=====================", 3);

            WriteCentered("=====================", 5);

            SetCursorAtSafe(halfW - 2, 4);
        }

        private void RenderQuestion()
        {
            Question question = QuestionLoader.Instance.CurrentQuestion;
            int w = Console.WindowWidth;
            int h = Console.WindowHeight;

            Console.Clear();

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
            int w = Console.WindowWidth;
            int h = Console.WindowHeight;
            int halfW = w / 2;
            int halfH = h / 2;

            Console.Clear();

            WriteCentered("Message:", 2);

            WriteCentered("=====================", 3);

            WriteCentered("=====================", 5);

            WriteMessageAtSafe(halfW - MessageToRender.Length / 2, 4, MessageToRender);
        }

        private void RenderOther()
        {

        }
    }
}
